using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Boid {
    class LineOfSightGoalSeekingBehaviour : GoalSeekingBehaviour {
        private const int minimumDistance = 5;
        private const int maximumDistance = 15;

        public LineOfSightGoalSeekingBehaviour(Boid boid, float viewingDistance, float minimumDistance)
            : base(boid, viewingDistance, minimumDistance) {
        }

        protected override void LineOfSightCheck() {
            Vector3 fromPosition = boid.transform.position + boid.EyeHeight;
            Vector3 toPosition = GoalNode.Position + new Vector3(0, Goal.Size.y, 0);

            RaycastHit hit = new RaycastHit();
            if (Physics.Linecast(fromPosition, toPosition, out hit)) {
                //Debug.Log("Object: " + hit.transform.name + ", distance: " + hit.distance);
                if (hit.transform.gameObject.name.Contains(Goal.Identifier)) {
                    if (hit.distance < Random.Range(minimumDistance, maximumDistance)) {
                        BehaviourComplete = true;
                    }
                }
            }
        }
    }
}
