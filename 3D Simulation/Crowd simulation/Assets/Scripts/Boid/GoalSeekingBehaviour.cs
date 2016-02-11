using System.Collections.Generic;
using Assets.Scripts.Environment.Navigation;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Boid {
    public class GoalSeekingBehaviour : FlockingBehaviour {

        private Node target;
        private Path path;
        protected Node goal;
        private const float targetMinimumDistance = 1.5f;

        public GoalSeekingBehaviour(global::Assets.Scripts.Boid.Boid boid, float viewingDistance, float minimumDistance) : base(boid, viewingDistance, minimumDistance) {
            this.boid = boid;
            this.MaxSpeed = 9.0f;
            this.MaxForce = 2.4f;
            this.VelocityDamping = 0.4f;
            this.SeparationFactor = 0.8f;
            BehaviourComplete = false;
        }

        public void Seek(Node goal, Graph graph) {
            Node startNode = graph.FindClosestNode(boid.transform.position);
            path = Path.Navigate(graph, startNode, goal);
            this.goal = goal;
        }

        public void Seek(Goal goal, Graph graph) {
            Node startNode = graph.FindClosestNode(boid.transform.position);
            Node goalNode = graph.FindClosestNode(goal.GameObject.transform.position);
            path = Path.Navigate(graph, startNode, goalNode);
            BehaviourComplete = false;
            this.goal = goalNode;
        }

        private Vector3 MoveAlongPath() {
            if (path != null) {
                if (this.target == null) {
                    this.target = path.FindClosestNode(boid.transform.position);
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
            if (Vector3.Distance(target.Position, this.boid.transform.position) < targetMinimumDistance) {
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

        public void ChooseClosestFromList<T>(List<T> goals) where T : Goal {
            if (goals.Count > 0) {
                Goal targetGoal = goals[(int)UnityEngine.Random.Range(0, goals.Count)];
                Seek(targetGoal, BootStrapper.EnvironmentManager.CurrentEnvironment.Graph);
            } else {
                BehaviourComplete = true; //Maybe really bad to say the goal is reached when there was never a goal?
            }
        }

        public void switchBehaviourToLoiter() {
            boid.behaviour = new LoiteringBehaviour(boid, path.Nodes[path.Nodes.Count - 2]);
        }

    }
}
