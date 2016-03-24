using UnityEngine;

namespace Assets.Scripts.Boid {
    internal class LineOfSightGoalSeekingBehaviour : GoalSeekingBehaviour {

        private const int MinimumDistance = 5;
        private const int MaximumDistance = 15;

        public LineOfSightGoalSeekingBehaviour(Boid boid) : base(boid) {}

        protected override void LineOfSightCheck() {
            Vector3 fromPosition = Boid.Position + Boid.EyeHeight;
            Vector3 toPosition = Goal.GameObject.transform.position;

            RaycastHit hit = new RaycastHit();
            if (isWithinLineOfSight(fromPosition, toPosition, ref hit) && isGoal(hit) && isCloseEnough(hit)) {
                BehaviourComplete = true;
                Boid.LookAt(toPosition);
            }
        }

        private static bool isCloseEnough(RaycastHit hit) {
            return hit.distance < Random.Range(MinimumDistance, MaximumDistance);
        }

        private bool isGoal(RaycastHit hit) {
            return hit.transform.gameObject.name.Contains(Goal.Identifier);
        }

        private static bool isWithinLineOfSight(Vector3 fromPosition, Vector3 toPosition, ref RaycastHit hit) {
            return Physics.Linecast(fromPosition, toPosition, out hit);
        }
    }
}
