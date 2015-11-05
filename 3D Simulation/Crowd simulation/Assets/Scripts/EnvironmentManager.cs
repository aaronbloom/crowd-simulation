﻿using UnityEngine;
using System.Collections;

public class EnvironmentManager : MonoBehaviour {

    public Vector3 Bounds;
    public Plane[] boundaries;

    private Plane[] constructCuboidBoundary (Vector3 bounds, Vector3 origin)
    {
        Plane[] boundaries = new Plane[6];
        boundaries[0] = new Plane(Vector3.up, origin);                  //bottom (ground) plane
        boundaries[1] = new Plane(Vector3.forward, origin);             //front
        boundaries[2] = new Plane(Vector3.right, origin);               //left
        boundaries[3] = new Plane(Vector3.up, origin + bounds);         //top
        boundaries[4] = new Plane(Vector3.forward, origin + bounds);    //back
        boundaries[5] = new Plane(Vector3.right, origin + bounds);      //right

        return boundaries;
    }

    // Use this for initialization
    void Start () {
        this.boundaries = this.constructCuboidBoundary(this.Bounds, Vector3.zero);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
