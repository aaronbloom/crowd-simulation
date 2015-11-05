﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlockingBehaviour : BoidBehaviour
{
    private Boid boid;
    private EnvironmentManager environmentManager;
    private float viewingDistance;
    private float minimumDistance;
    public FlockingBehaviour (Boid boid, float viewingDistance, float minimumDistance)
    {
        this.boid = boid;
        this.viewingDistance = viewingDistance;
        this.minimumDistance = minimumDistance;
        this.environmentManager = EnvironmentManager.Shared();
    }

    public Vector3 updateAcceleration ()
    {
        List<Boid> boids = FindBoidsWithinView();

        Vector3 cohesionDirection = Cohesion(boids);
        Vector3 seperationDirection = Separation(boids);
        Vector3 alignmentDirection = Alignment(boids);
        Vector3 boundaryAvoidance = PlaneAvoidance();

        Vector3 acceleration = Vector3.zero;
        acceleration += seperationDirection;
        acceleration += alignmentDirection;
        acceleration += cohesionDirection;
        acceleration += boundaryAvoidance;

        return acceleration;
    }

    private Vector3 PlaneAvoidance()
    {
        Plane[] boundaries = environmentManager.Boundaries;
        Vector3 steeringDirection = Vector3.zero;
        foreach (Plane boundary in boundaries)
        {
            //if really close proximity to a plane boundary
            if (boundary.GetDistanceToPoint(boid.transform.position) < this.minimumDistance)
            {
                Vector3 avoidDirection = boundary.normal;
                avoidDirection *= boid.maxSpeed;
                avoidDirection = Vector3.ClampMagnitude(avoidDirection, boid.maxForce);
                steeringDirection += avoidDirection;
            }

            //if directly facing boundary and within reasonable distance from plane boundary
            Ray direction = new Ray(boid.transform.position, boid.velocity);
            float distance;
            if (boundary.Raycast(direction, out distance))
            {
                if (distance < 35)
                {
                    Vector3 avoidDirection = boundary.normal;
                    avoidDirection *= boid.maxSpeed;
                    avoidDirection = Vector3.ClampMagnitude(avoidDirection, boid.maxForce);
                    steeringDirection += avoidDirection;
                }
            }
        }
        return steeringDirection;
    }

    private List<Boid> FindBoidsWithinView()
    {
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");
        List<Boid> closeBoids = new List<Boid>();

        foreach (GameObject otherBoid in boids)
        {
            float distance = Vector3.Distance(boid.transform.position, otherBoid.transform.position);
            if (distance < this.viewingDistance && distance != 0)
            {
                closeBoids.Add(otherBoid.GetComponent<Boid>());
            }
        }

        return closeBoids;
    }

    private Vector3 Cohesion(List<Boid> boids)
    {
        Vector3 averagePosition = Vector3.zero;
        foreach (Boid otherBoid in boids)
        {
            averagePosition += otherBoid.transform.position;
        }
        if (boids.Count > 0)
        {
            averagePosition /= boids.Count;
            Vector3 aim = averagePosition - boid.transform.position;
            aim.Normalize();
            aim *= boid.maxSpeed;
            Vector3 steeringDirection = aim - boid.velocity;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, boid.maxForce);
            return steeringDirection;
        }
        return Vector3.zero;
    }

    private Vector3 Alignment(List<Boid> boids)
    {
        Vector3 averageHeading = Vector3.zero;
        foreach (Boid otherBoid in boids)
        {
            averageHeading += otherBoid.velocity;
        }
        if (boids.Count > 0)
        {
            averageHeading /= boids.Count;
            averageHeading.Normalize();
            averageHeading *= boid.maxSpeed;
            Vector3 steeringDirection = averageHeading - boid.velocity;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, boid.maxForce);
            return steeringDirection;
        }
        return Vector3.zero;
    }

    private Vector3 Separation(List<Boid> boids)
    {
        int count = 0;
        Vector3 steeringDirection = Vector3.zero;
        foreach (Boid otherBoid in boids)
        {
            float distance = Vector3.Distance(boid.transform.position, otherBoid.transform.position);
            if (distance < minimumDistance)
            {
                count++;
                Vector3 difference = boid.transform.position - otherBoid.transform.position;
                difference.Normalize();
                difference /= distance; //weight by distance
                steeringDirection += difference;
            }
        }
        if (count > 0)
        {
            steeringDirection /= count;
        }
        if (steeringDirection.magnitude > 0)
        {
            steeringDirection.Normalize();
            steeringDirection *= boid.maxSpeed;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, boid.maxForce);
        }
        return steeringDirection;
    }
}