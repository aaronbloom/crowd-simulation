using UnityEngine;

namespace Assets.Scripts.Boid {
    internal class LineOfSightGoalSeekingBehaviour : GoalSeekingBehaviour {
        private const int minimumDistance = 5;
        private const int maximumDistance = 15;

        public LineOfSightGoalSeekingBehaviour(Boid boid)
            : base(boid) {
        }

        protected override void LineOfSightCheck() {
            Vector3 fromPosition = boid.Position + boid.EyeHeight;
            Vector3 toPosition = Goal.GameObject.transform.position;

            RaycastHit hit = new RaycastHit();
            if (Physics.Linecast(fromPosition, toPosition, out hit)) {
                //Debug.Log("Object: " + hit.transform.name + ", distance: " + hit.distance);
                if (hit.transform.gameObject.name.Contains(Goal.Identifier)) {
                    if (hit.distance < Random.Range(minimumDistance, maximumDistance)) {
                        BehaviourComplete = true;
                        boid.LookAt(toPosition); //look at stage once found
                    }
                }
            }
        }
    }
}
