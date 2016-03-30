using UnityEngine;

namespace Assets.Scripts.Boid {

    /// <summary>
    /// Line of sight goal seeking behaviour
    /// </summary>
    internal class LineOfSightGoalSeekingBehaviour : GoalSeekingBehaviour {

        private const int MinimumDistance = 5;
        private const int MaximumDistance = 15;

        public LineOfSightGoalSeekingBehaviour(Boid boid) : base(boid) {}

        /// <summary>
        /// Checks to see if goal is within line of sight, if so then the behaviour is satisfied
        /// </summary>
        protected override void LineOfSightCheck() {
            Vector3 fromPosition = Boid.Position + Boid.EyeHeight;
            Vector3 toPosition = Goal.GameObject.transform.position;

            RaycastHit hit = new RaycastHit();
            if (isWithinLineOfSight(fromPosition, toPosition, ref hit) && isGoal(hit) && isCloseEnough(hit.distance)) {
                BehaviourComplete = true;
                Boid.LookAt(toPosition);
            }
        }

        /// <summary>
        /// Checks to see if the boid is close enough to the goal (line of sight).
        /// Can the boid current "see" the goal.
        /// </summary>
        /// <param name="distance">How far away the goal is</param>
        /// <returns></returns>
        private static bool isCloseEnough(float distance) {
            return distance < Random.Range(MinimumDistance, MaximumDistance);
        }

        /// <summary>
        /// Checks to see if a raycast "hit" has found the goal
        /// </summary>
        /// <param name="hit">The raycast outcome</param>
        /// <returns></returns>
        private bool isGoal(RaycastHit hit) {
            return hit.transform.gameObject.name.Contains(Goal.Identifier);
        }

        /// <summary>
        /// Does a raycast between two positions hit anything (GameObjects)? If so, the ref param "hit" is populated. 
        /// </summary>
        /// <param name="fromPosition">starting position</param>
        /// <param name="toPosition">end poisition</param>
        /// <param name="hit">retuned hit value</param>
        /// <returns>bool, has the raycast hit anything</returns>
        private static bool isWithinLineOfSight(Vector3 fromPosition, Vector3 toPosition, ref RaycastHit hit) {
            return Physics.Linecast(fromPosition, toPosition, out hit);
        }
    }
}
