﻿using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour {

    private static EnvironmentManager shared;
    public Vector3 Bounds;
    private Plane[] boundaries;
    public Plane[] Boundaries
    {
        get { return boundaries; }
    }

    private Plane[] constructCuboidBoundary (Vector3 bounds, Vector3 origin)
    {
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
    void Start () {
        this.boundaries = this.constructCuboidBoundary(this.Bounds, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    // Setup the static instance of this EnvironmentManager
    private void Awake()
    {
        // Set the 'shared' variable as this environment manager
        if (shared == null)
        {
            shared = this;
        }
        else
        {
            Destroy(this);
        }
    }

    // Return the static instance of this EnvironmentManager
    public static EnvironmentManager Shared()
    {
        return shared;
    }
}
