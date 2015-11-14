using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour {

    public Vector3 Bounds { get; private set; }

    private static EnvironmentManager shared;

    public Plane[] Boundaries { get; private set; }
 

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

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    // Setup the static instance of this EnvironmentManager
    private void Awake() {
        Bounds = new Vector3(100, 100, 100);
        this.Boundaries = constructCuboidBoundary(this.Bounds, Vector3.zero);

        // Set the 'shared' variable as this environment manager
        if (shared == null) {
            shared = this;
        } else {
            Destroy(this);
        }
    }

    // Return the static instance of this EnvironmentManager
    public static EnvironmentManager Shared() {
        return shared;
    }
}
