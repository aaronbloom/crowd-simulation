using UnityEngine;
using System.Collections;

public class EnvironmentManager {

    public Vector3 Bounds { get; private set; }
    public Plane[] Boundaries { get; private set; }
    public Vector3 Origin { get; private set; }
    public Vector3 EnvironmentCenter {
        get {
            return (Bounds / 2) + Origin;
        }
    }

    private static EnvironmentManager shared;

    // Setup the static instance of this EnvironmentManager
    public EnvironmentManager() {
        initSingleton();
        Origin = Vector3.zero;
        Bounds = new Vector3(100, 100, 100);
        this.Boundaries = constructCuboidBoundary(this.Bounds, Origin);
    }

    private void initSingleton() {
        // Set the 'shared' variable as this environment manager
        if (shared == null) {
            shared = this;
        } else {
            throw new System.InvalidOperationException("Singleton already setup");
        }
    }

    // Return the static instance of this EnvironmentManager
    public static EnvironmentManager Shared() {
        return shared;
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
