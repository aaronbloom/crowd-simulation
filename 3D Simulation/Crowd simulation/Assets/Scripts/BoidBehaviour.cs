using UnityEngine;
using System.Collections;

public abstract class BoidBehaviour {

    public float MaxSpeed { get; protected set; }
    public float MaxForce { get; protected set; }

    public abstract Vector3 InitialVelocity();

    public abstract Vector3 updateAcceleration();

    public abstract void DrawGraphGizmo();
}
