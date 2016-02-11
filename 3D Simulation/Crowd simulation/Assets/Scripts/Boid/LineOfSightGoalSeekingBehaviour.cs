using UnityEngine;

namespace Assets.Scripts.Boid {
    class LineOfSightGoalSeekingBehaviour : GoalSeekingBehaviour {

        private readonly string goalType;
        private const int minimumDistance = 4;
        private const int maximumDistance = 10;

        public LineOfSightGoalSeekingBehaviour(Boid boid, float viewingDistance, float minimumDistance, string goalType)
            : base(boid, viewingDistance, minimumDistance) {
            this.goalType = goalType;
        }

        protected override void LineOfSightCheck() {
            Vector3 fromPosition = boid.transform.position + boid.EyeHeight;
            Vector3 toPosition = goal.Position;

            RaycastHit hit = new RaycastHit();
            if (Physics.Linecast(fromPosition, toPosition, out hit)) {
                //Debug.Log("Object: " + hit.transform.name + ", distance: " + hit.distance);
                if (hit.transform.gameObject.name.Contains(goalType)) {
                    if (hit.distance < Random.Range(minimumDistance, maximumDistance)) {
                        BehaviourComplete = true;
                    }
                }
            }
        }
    }
}
