using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FlockingBehaviour : BoidBehaviour {

    private static readonly string BoidTag = "Boid";

    private Boid boid;
    private EnvironmentManager environmentManager;
    private float viewingDistance;
    private float minimumDistance;
    private readonly float seperationFactor = 1.5f;

    public FlockingBehaviour(Boid boid, float viewingDistance, float minimumDistance) {
        this.boid = boid;
        this.viewingDistance = viewingDistance;
        this.minimumDistance = minimumDistance;
        this.environmentManager = EnvironmentManager.Shared();
    }

    public override Vector3 updateAcceleration() {
        List<Boid> boids = FindBoidsWithinView();

        Vector3 cohesionDirection = Cohesion(boids);
        Vector3 seperationDirection = Separation(boids);
        Vector3 alignmentDirection = Alignment(boids);
        Vector3 boundaryAvoidance = PlaneAvoidance();

        Vector3 acceleration = Vector3.zero;
        acceleration += seperationDirection * seperationFactor;
        acceleration += alignmentDirection;
        acceleration += cohesionDirection;
        acceleration += boundaryAvoidance;

        return acceleration;
    }

    private List<Boid> FindBoidsWithinView() {
        GameObject[] boids = GameObject.FindGameObjectsWithTag(BoidTag);
        List<Boid> closeBoids = new List<Boid>();
        foreach (GameObject otherBoid in boids) {
            if (!object.ReferenceEquals(this.boid, otherBoid) && isWithinView(boid, otherBoid)) {
                closeBoids.Add(otherBoid.GetComponent<Boid>());
            }
        }
        return closeBoids;
    }

    private bool isWithinView(Boid boid, GameObject otherBoid) {
        Vector3 boidPosition = boid.transform.position;
        Vector3 otherBoidPosition = otherBoid.transform.position;
        float distance = Vector3.Distance(boidPosition, otherBoidPosition);
        return distance < this.viewingDistance && distance != 0;
    }

    private Vector3 Cohesion(List<Boid> boids) {
        if (boids.Count > 0) {
            Vector3 averagePosition = getAveragePosition(boids);
            Vector3 aim = averagePosition - boid.transform.position;
            aim.Normalize();
            aim *= Boid.MaxSpeed;
            Vector3 steeringDirection = aim - boid.Velocity;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, Boid.MaxForce);
            return steeringDirection;
        }
        return Vector3.zero;
    }

    private static Vector3 getAveragePosition(List<Boid> boids) {
        Vector3 averagePosition = Vector3.zero;

        foreach (Boid otherBoid in boids) {
            averagePosition += otherBoid.transform.position;
        }

        return averagePosition / boids.Count;
    }

    private Vector3 Separation(List<Boid> boids) {
        Vector3 steeringDirectionAggregator = Vector3.zero;
        int count = 0;
        foreach (Boid otherBoid in boids) {
            count++;
            steeringDirectionAggregator += calculateSteeringDirection(otherBoid);
        }
        return calculateAverageSteeringDirection(steeringDirectionAggregator, count);
    }

    private Vector3 calculateSteeringDirection(Boid otherBoid) {
        Vector3 steeringDirection = Vector3.zero;
        float distance = Vector3.Distance(boid.transform.position, otherBoid.transform.position);
        if (distance < minimumDistance) {
            Vector3 difference = boid.transform.position - otherBoid.transform.position;
            difference.Normalize();
            difference /= distance; //weight by distance
            steeringDirection += difference;
        }
        return steeringDirection;
    }

    private static Vector3 calculateAverageSteeringDirection(Vector3 steeringDirectionAggregator, int count) {
        Vector3 averageSteeringDirection = steeringDirectionAggregator;
        if (count > 0) {
            averageSteeringDirection /= count;
        }
        if (averageSteeringDirection.magnitude > 0) {
            averageSteeringDirection.Normalize();
            averageSteeringDirection *= Boid.MaxSpeed;
            averageSteeringDirection = Vector3.ClampMagnitude(steeringDirectionAggregator, Boid.MaxForce);
        }
        return averageSteeringDirection;
    }

    private Vector3 Alignment(List<Boid> boids) {
        if (boids.Count > 0) {
            Vector3 averageHeading = getAverageHeading(boids);
            averageHeading.Normalize();
            averageHeading *= Boid.MaxSpeed;
            Vector3 steeringDirection = averageHeading - boid.Velocity;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, Boid.MaxForce);
            return steeringDirection;
        }
        return Vector3.zero;
    }

    private static Vector3 getAverageHeading(List<Boid> boids) {
        Vector3 averageHeading = Vector3.zero;
        foreach (Boid otherBoid in boids) {
            averageHeading += otherBoid.Velocity;
        }
        averageHeading /= boids.Count;
        return averageHeading;
    }

    private Vector3 PlaneAvoidance() {
        Plane[] boundaries = environmentManager.Boundaries;
        Vector3 steeringDirection = Vector3.zero;
        foreach (Plane boundary in boundaries) {
            //if really close proximity to a plane boundary
            if (boundary.GetDistanceToPoint(boid.transform.position) < this.minimumDistance) {
                Vector3 avoidDirection = boundary.normal;
                avoidDirection *= Boid.MaxSpeed;
                avoidDirection = Vector3.ClampMagnitude(avoidDirection, Boid.MaxForce);
                steeringDirection += avoidDirection;
            }

            //if directly facing boundary and within reasonable distance from plane boundary
            Ray direction = new Ray(boid.transform.position, boid.Velocity);
            float distance;
            if (boundary.Raycast(direction, out distance)) {
                if (distance < 35) {
                    Vector3 avoidDirection = boundary.normal;
                    avoidDirection *= Boid.MaxSpeed;
                    avoidDirection = Vector3.ClampMagnitude(avoidDirection, Boid.MaxForce);
                    steeringDirection += avoidDirection;
                }
            }
        }
        return steeringDirection;
    }
}
