using System;
using System.Collections.Generic;
using Assets.Scripts.Environment.Navigation;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    public class GoalSeekingBehaviour : FlockingBehaviour {

        protected Goal Goal;

        private const int NodeReachedTimeout = 5; //given in seconds
        private const float TargetMinimumDistance = 1.5f;
        private Node target;
        private Path path;
        private Graph graph;
        private DateTime hitLastNode;

        /// <summary>
        /// Creates a new Goal seeking behaviour
        /// </summary>
        /// <param name="boid"></param>
        public GoalSeekingBehaviour(Boid boid) : base(boid) {
            this.Boid = boid;
            this.MaxForce = 2.4f;
            this.VelocityDamping = 0.4f;
            this.SeparationFactor = 1.4f;
            BehaviourComplete = false;
        }

        /// <summary>
        /// Find a navigation path to the <paramref name="goal"/>, along the <paramref name="navGraph"/>.
        /// </summary>
        /// <param name="goal">The goal to reach</param>
        /// <param name="navGraph">The graph to navigate</param>
        public void Seek(Goal goal, Graph navGraph) {
            Node startNode = navGraph.FindClosestNode(Boid.Position);
            Node goalNode = navGraph.FindClosestNode(goal.FrontPosition());
            path = Path.Navigate(navGraph, startNode, goalNode);
            BehaviourComplete = false;
            this.Goal = goal;
            this.graph = navGraph;
            hitLastNode = DateTime.Now;
        }

        /// <summary>
        /// Returns the behaviours initial velocity.
        /// </summary>
        /// <returns>Initial velocity</returns>
        public override Vector3 InitialVelocity() {
            return Vector3.zero;
        }

        /// <summary>
        /// Returns the acceletation for one increment of the behaviour.
        /// </summary>
        /// <returns>Acceleration</returns>
        public override Vector3 UpdateAcceleration() {
            if (!BehaviourComplete) {
                List<Boid> boids = FindBoidsWithinView();

                Vector3 seperationDirection = Separation(boids);

                Vector3 acceleration = Vector3.zero;
                acceleration += seperationDirection*SeparationFactor;
                acceleration += moveAlongPath();
                return acceleration;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Debug visual gizmo drawing.
        /// </summary>
        public override void DrawGraphGizmo() {
            Gizmos.color = Color.green;
            path.DrawGraphGizmo();
        }

        /// <summary>
        /// Finds the next behaviours goal randomly from the <paramref name="goals"/> of type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">A sub-class of Goal (WorldObject)</typeparam>
        /// <param name="goals">List of goals</param>
        /// <returns></returns>
        public T ChooseClosestFromList<T>(List<T> goals) where T : Goal {
            if (goals.Count > 0) {
                Goal targetGoal = goals[Random.Range(0, goals.Count)];
                Seek(targetGoal, BootStrapper.EnvironmentManager.CurrentEnvironment.Graph);
                return (T) targetGoal;
            }
            BehaviourComplete = true; //Maybe really bad to say the goal is reached when there was never a goal?
            return null;
        }

        protected virtual void LineOfSightCheck() {}

        private void targetNextNodeAlongPath() {
            if (hitLastNode < DateTime.Now.AddSeconds(-NodeReachedTimeout)) {
                reseek(); //recalc after time
            } else {
                if (Vector3.Distance(target.Position, this.Boid.Position) < TargetMinimumDistance) {
                    hitLastNode = DateTime.Now;
                    int index = path.Nodes.IndexOf(this.target) + 1;
                    this.LineOfSightCheck();
                    if (index < path.Nodes.Count - 1) {
                        this.target = path.Nodes[index];
                    } else {
                        //Goal Found
                        BehaviourComplete = true;
                    }
                }
            }
        }

        private void reseek() {
            Debug.Log("Recalculating Path: " + Boid.Name);
            target = null;
            Seek(Goal, graph);
        }

        private Vector3 moveAlongPath() {
            if (path != null) {
                if (this.target == null) {
                    this.target = path.FindClosestNode(Boid.Position);
                }
                this.targetNextNodeAlongPath();
                if (this.target == null) return Vector3.zero;
                return this.SteerTowardsPoint(this.target.Position);
            }
            return Vector3.zero;
        }
    }
}
