using UnityEngine;

namespace Assets.Scripts.Boid {

    /// <summary>
    /// Abstract base boid behaviour class
    /// </summary>
    public abstract class BoidBehaviour
    {

        protected Boid Boid;

        public float MaxForce { get; protected set; }
        public bool BehaviourComplete { get; protected set; }

        public float VelocityDamping { get; protected set; }

        /// <summary>
        /// Returns boid behaviours initial velocity
        /// </summary>
        /// <returns>Initial velocity</returns>
        public abstract Vector3 InitialVelocity();

        /// <summary>
        /// Calculates and returns one ticks worth of acceleration
        /// </summary>
        /// <returns>Acceleration</returns>
        public abstract Vector3 UpdateAcceleration();

        /// <summary>
        /// Debug gizmo drawing
        /// </summary>
        public abstract void DrawGraphGizmo();

        /// <summary>
        /// Calculates the force onto a boid to steer towards a target position.
        /// </summary>
        /// <param name="target">Target position</param>
        /// <returns>Steering force</returns>
        protected Vector3 SteerTowardsPoint(Vector3 target) {
            Vector3 aim = target - Boid.Position;
            aim.Normalize();
            aim *= Boid.Properties.MoveSpeed;
            Vector3 steeringDirection = aim - Boid.Velocity;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, MaxForce);
            return steeringDirection;
        }
    }
}
