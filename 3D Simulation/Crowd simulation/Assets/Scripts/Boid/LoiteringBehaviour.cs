using Assets.Scripts.Environment.Navigation;
using UnityEngine;

namespace Assets.Scripts.Boid {
    public class LoiteringBehaviour : BoidBehaviour {

        private Node home;
        private Node target;
        private Path path;

        public LoiteringBehaviour(global::Assets.Scripts.Boid.Boid boid, Node home) {
            this.boid = boid;
            this.MaxSpeed = 8.0f;
            this.MaxForce = 0.5f;
            this.VelocityDamping = 0.5f;
            this.home = home;
        }

        public override void DrawGraphGizmo() {
            Gizmos.color = Color.green;
            path.DrawGraphGizmo();
        }

        public override Vector3 InitialVelocity() {
            return Vector3.zero;
        }

        public override Vector3 updateAcceleration() {
            Vector3 acceleration = Vector3.zero;
            acceleration += MoveAlongPath();
            return acceleration;
        }

        private void createLoiterPath() {
            this.path = Path.Loiter(BootStrapper.EnvironmentManager.CurrentEnvironment.Graph, home, 8, 3, 20);
        }

        private Vector3 MoveAlongPath() {
            if (path == null) {
                createLoiterPath();
            }
            if (this.target == null) {
                this.target = path.Nodes[0];
                return MoveAlongPath();
            } else {
                this.TargetNextNodeAlongPath();
                if (this.target == null) return Vector3.zero;
                return this.SteerTowardsPoint(this.target.Position);
            }
        }

        private void TargetNextNodeAlongPath() {
            if (Vector3.Distance(target.Position, this.boid.Position) < 1) {
                int index = path.Nodes.IndexOf(this.target) + 1;
                if (index < path.Nodes.Count - 1) {
                    this.target = path.Nodes[index];
                } else {
                    switchBehaviourToGoalSeeking();
                }
            }
        }

        public void switchBehaviourToGoalSeeking() {
            boid.behaviour = new GoalSeekingBehaviour(boid, 10f, 2.5f);
        }


    }
}
