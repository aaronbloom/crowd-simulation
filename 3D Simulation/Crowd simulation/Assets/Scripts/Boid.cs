﻿using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour {

    private float viewingDistance = 20;
    private float minimumDistance = 5;
    public Vector3 velocity;
    private Vector3 acceleration;
    private float maxSpeed = 8.0f;
    private float maxForce = 0.05f;
    
	// Use this for initialization
	void Start () {
        this.velocity = Vector3.zero;
	}

    List<Boid> findBoidsWithinView()
    {
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");
        List<Boid> closeBoids = new List<Boid>();

        foreach(GameObject boid in boids)
        {
            float distance = Vector3.Distance(boid.transform.position, this.transform.position);
            if (distance < this.viewingDistance && distance != 0)
            {
                closeBoids.Add(boid.GetComponent<Boid>());
            }
        }

        return closeBoids;
    }


    Vector3 Alignment (List<Boid> boids)
    {
        Vector3 averageHeading = Vector3.zero;
        foreach (Boid boid in boids)
        {
            averageHeading += boid.velocity;
        }
        if (boids.Count > 0)
        {
            averageHeading /= boids.Count;
            averageHeading.Normalize();
            averageHeading *= this.maxSpeed;
            Vector3 steeringDirection = averageHeading - this.velocity;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, this.maxForce);
            return steeringDirection;
        }
        return Vector3.zero;
    }


    // Update is called once per frame
    void Update () {
        List<Boid> boids = findBoidsWithinView();

        Vector3 alignmentDirection = Alignment(boids);

        this.acceleration += alignmentDirection;

        this.velocity += acceleration;
        this.velocity = Vector3.ClampMagnitude(this.velocity, maxSpeed);
        this.transform.position += (this.velocity * Time.deltaTime);
        this.acceleration = Vector3.zero; //reset acceleration
    }
}
