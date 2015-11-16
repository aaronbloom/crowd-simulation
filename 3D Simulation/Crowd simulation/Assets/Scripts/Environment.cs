using UnityEngine;
using System.Collections;

public class Environment {

    public Vector3 Bounds { get; private set; }
    public Plane[] Boundaries { get; private set; }
    public Vector3 Origin { get; private set; }
    public Vector3 EnvironmentCenter {
        get {
            return (Bounds / 2) + Origin;
        }
    }
    public Graph graph { get; private set; }

    public Environment() {
        Origin = Vector3.zero;
        Bounds = new Vector3(100, 100, 100);
        this.Boundaries = constructCuboidBoundary(this.Bounds, Origin);
        this.graph = Graph.ConstructGraph(this);
    }

    public Vector2 GetFloorDimentions() {
        return new Vector2(Bounds.x - Origin.x, Bounds.z - Origin.z);
    }

    private Plane[] constructCuboidBoundary(Vector3 bounds, Vector3 origin) {
        Plane[] boundaries = new Plane[6];
        boundaries[0] = new Plane(Vector3.up, origin);                  //bottom (ground) plane
        boundaries[1] = new Plane(Vector3.forward, origin);             //front
        boundaries[2] = new Plane(Vector3.right, origin);               //left
        boundaries[3] = new Plane(Vector3.down, origin + bounds);       //top
        boundaries[4] = new Plane(Vector3.back, origin + bounds);       //back
        boundaries[5] = new Plane(Vector3.left, origin + bounds);       //right

        return boundaries;
    }
}
