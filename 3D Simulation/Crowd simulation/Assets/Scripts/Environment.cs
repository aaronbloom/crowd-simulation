using UnityEngine;
using System.Collections;
using Assets.Scripts;

public class Environment {

    public World World { get; private set; }
    public Graph Graph { get; private set; }
    public Vector3 Bounds { get; private set; }
    public Plane[] Boundaries { get; private set; }
    public Vector3 Origin { get; private set; }
    public Vector3 EnvironmentCenter {
        get {
            return (Bounds / 2) + Origin;
        }
    }
    public Vector2 FloorDimentions {
        get {
            return new Vector2(Bounds.x - Origin.x, Bounds.z - Origin.z);
        }
    }


    public Environment() {
        this.Origin = Vector3.zero;
        this.Bounds = new Vector3(100, 50, 100); //width, height, length
        this.Boundaries = constructCuboidBoundary(this.Bounds, Origin);
        this.World = new World();
    }

    public void Build() {
        constructNavMesh();
    }

    private void constructNavMesh() {
        this.Graph = Graph.ConstructGraph(this, 1f);
        foreach (var collidable in World.Walls) {
            this.Graph.Cull(collidable);
        }
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
