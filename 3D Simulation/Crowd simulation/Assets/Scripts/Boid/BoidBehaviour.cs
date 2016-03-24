using UnityEngine;

namespace Assets.Scripts.Boid {
    public abstract class BoidBehaviour
    {

        protected Boid Boid;

        public float MaxForce { get; protected set; }
        public bool BehaviourComplete { get; protected set; }

        public float VelocityDamping { get; protected set; }

        public abstract Vector3 InitialVelocity();

        public abstract Vector3 UpdateAcceleration();

        public abstract void DrawGraphGizmo();

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
