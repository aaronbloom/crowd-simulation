using UnityEngine;
using System.Collections;

public abstract class BoidBehaviour {

    protected Boid boid;

    public float MaxSpeed { get; protected set; }
    public float MaxForce { get; protected set; }

    public float VelocityDamping { get; protected set; }

    public abstract Vector3 InitialVelocity();

    public abstract Vector3 updateAcceleration();

    public abstract void DrawGraphGizmo();

    protected Vector3 SteerTowardsPoint(Vector3 target) {
        Vector3 aim = target - boid.transform.position;
        aim.Normalize();
        aim *= MaxSpeed;
        Vector3 steeringDirection = aim - boid.Velocity;
        steeringDirection = Vector3.ClampMagnitude(steeringDirection, MaxForce);
        return steeringDirection;
    }
}
