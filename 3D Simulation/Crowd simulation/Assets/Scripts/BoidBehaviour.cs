using UnityEngine;
using System.Collections;

public abstract class BoidBehaviour {
    public abstract Vector3 updateAcceleration();

    public abstract void DrawGraphGizmo();
}
