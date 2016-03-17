using System;
using System.Collections.Generic;
using Assets.Scripts.Environment.Navigation;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Boid {
    public class GoalSeekingBehaviour : FlockingBehaviour {

        public Node GoalNode { get; protected set; }
        public Goal Goal { get; protected set; }

        private Node target;
        private Path path;
        private Graph graph;
        private DateTime hitLastNode;
        private int nodeReachedTimeout = 5; //given in seconds
        private const float targetMinimumDistance = 1.5f;

        public GoalSeekingBehaviour(Boid boid) : base(boid) {
            this.boid = boid;
            this.MaxForce = 2.4f;
            this.VelocityDamping = 0.4f;
            this.SeparationFactor = 0.8f;
            BehaviourComplete = false;
        }

        public void Seek(Goal goal, Graph navGraph) {
            Node startNode = navGraph.FindClosestNode(boid.Position);
            Node goalNode = navGraph.FindClosestNode(goal.FrontPosition());
            path = Path.Navigate(navGraph, startNode, goalNode);
            BehaviourComplete = false;
            this.GoalNode = goalNode;
            this.Goal = goal;
            this.graph = navGraph;
            hitLastNode = DateTime.Now;
        }

        private void reseek() {
            Debug.Log("Recalculating Path: " + boid.Name);
            target = null;
            Seek(Goal, graph);
        }

        private Vector3 MoveAlongPath() {
            if (path != null) {
                if (this.target == null) {
                    this.target = path.FindClosestNode(boid.Position);
                }
                this.TargetNextNodeAlongPath();
                if (this.target == null) return Vector3.zero;
                return this.SteerTowardsPoint(this.target.Position);
            }
            return Vector3.zero;
        }

        public override Vector3 InitialVelocity() {
            return Vector3.zero;
        }

        public override Vector3 updateAcceleration() {
            if (!BehaviourComplete) {
                List<global::Assets.Scripts.Boid.Boid> boids = FindBoidsWithinView();

                Vector3 seperationDirection = Separation(boids);

                Vector3 acceleration = Vector3.zero;
                acceleration += seperationDirection*SeparationFactor;
                acceleration += MoveAlongPath();
                return acceleration;
            }
            return Vector3.zero;
        }

        public override void DrawGraphGizmo() {
            Gizmos.color = Color.green;
            path.DrawGraphGizmo();
        }

        protected virtual void LineOfSightCheck() {}

        private void TargetNextNodeAlongPath() {
            if (hitLastNode < DateTime.Now.AddSeconds(-nodeReachedTimeout)) {
                reseek(); //recalc after time
            } else {
                if (Vector3.Distance(target.Position, this.boid.Position) < targetMinimumDistance) {
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

        public void chooseNewGoal() {
            //Do something more intelligent here.            
            List<Goal> goals = BootStrapper.EnvironmentManager.CurrentEnvironment.World.Goals;
            if (goals.Count > 0) {
                Goal targetGoal = goals[(int)UnityEngine.Random.Range(0, goals.Count)];
                Seek(targetGoal, BootStrapper.EnvironmentManager.CurrentEnvironment.Graph);
            } else {
                BehaviourComplete = true; //Maybe really bad to say the goal is reached when there was never a goal?
            }
        }

        public T ChooseClosestFromList<T>(List<T> goals) where T : Goal {
            if (goals.Count > 0) {
                Goal targetGoal = goals[(int)UnityEngine.Random.Range(0, goals.Count)];
                Seek(targetGoal, BootStrapper.EnvironmentManager.CurrentEnvironment.Graph);
                return (T) targetGoal;
            } else {
                BehaviourComplete = true; //Maybe really bad to say the goal is reached when there was never a goal?
            }
            return null;
        }

        public void ChooseClosestFromList<T>(List<T> goals, String constraint) where T : Goal {
            if (goals.Count > 0) {
                Goal targetGoal = goals[(int) UnityEngine.Random.Range(0, goals.Count)]; ;
                while (targetGoal.Tag != constraint) {
                    targetGoal = goals[(int) UnityEngine.Random.Range(0, goals.Count)];
                }
                Seek(targetGoal, BootStrapper.EnvironmentManager.CurrentEnvironment.Graph);
            } else {
                BehaviourComplete = true; //Maybe really bad to say the goal is reached when there was never a goal?
            }
        }
    }
}
