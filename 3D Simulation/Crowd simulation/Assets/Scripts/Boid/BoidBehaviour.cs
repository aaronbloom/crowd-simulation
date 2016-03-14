using UnityEngine;

namespace Assets.Scripts.Boid {
    public abstract class BoidBehaviour
    {

        protected global::Assets.Scripts.Boid.Boid boid;

        public float MaxForce { get; protected set; }
        public bool BehaviourComplete { get; protected set; }

        public float VelocityDamping { get; protected set; }

        public abstract Vector3 InitialVelocity();

        public abstract Vector3 updateAcceleration();

        public abstract void DrawGraphGizmo();

        protected Vector3 SteerTowardsPoint(Vector3 target) {
            Vector3 aim = target - boid.Position;
            aim.Normalize();
            aim *= boid.Properties.Speed;
            Vector3 steeringDirection = aim - boid.Velocity;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, MaxForce);
            return steeringDirection;
        }
    }
}
