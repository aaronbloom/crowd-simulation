using System;
using System.Collections.Generic;
using Assets.Scripts.Environment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    public class FlockingBehaviour : BoidBehaviour {

        private static readonly string BoidTag = "Boid";

        private EnvironmentManager environmentManager;
        public float SeparationFactor { get; protected set; }

        public FlockingBehaviour(Boid boid) {
            this.MaxForce = 0.05f;
            this.VelocityDamping = 1f;
            this.SeparationFactor = 1.5f;
            this.boid = boid;
            this.environmentManager = EnvironmentManager.Shared();
        }

        public override Vector3 InitialVelocity() {
            float MaxSpeed = boid.Properties.Speed;
            return Random.onUnitSphere * Random.Range(MaxSpeed / 2, MaxSpeed);
        }

        public override Vector3 updateAcceleration() {
            List<global::Assets.Scripts.Boid.Boid> boids = FindBoidsWithinView();

            Vector3 cohesionDirection = Cohesion(boids);
            Vector3 seperationDirection = Separation(boids);
            Vector3 alignmentDirection = Alignment(boids);
            Vector3 boundaryAvoidance = PlaneAvoidance();

            Vector3 acceleration = Vector3.zero;
            acceleration += seperationDirection * SeparationFactor;
            acceleration += alignmentDirection;
            acceleration += cohesionDirection;
            acceleration += boundaryAvoidance;

            return acceleration;
        }

        public override void DrawGraphGizmo() {
            throw new NotImplementedException();
        }

        protected List<global::Assets.Scripts.Boid.Boid> FindBoidsWithinView() {
            GameObject[] boids = GameObject.FindGameObjectsWithTag(BoidTag);
            List<global::Assets.Scripts.Boid.Boid> closeBoids = new List<global::Assets.Scripts.Boid.Boid>();
            foreach (GameObject otherBoid in boids) {
                if (!object.ReferenceEquals(this.boid, otherBoid) && isWithinView(boid, otherBoid)) {
                    closeBoids.Add(BootStrapper.BoidManager.FindBoid(otherBoid));
                }
            }
            return closeBoids;
        }

        private bool isWithinView(global::Assets.Scripts.Boid.Boid boid, GameObject otherBoid) {
            Vector3 boidPosition = boid.Position;
            Vector3 otherBoidPosition = otherBoid.transform.position;
            float distance = Vector3.Distance(boidPosition, otherBoidPosition);
            return distance < boid.ViewingDistance && distance != 0;
        }

        protected Vector3 Cohesion(List<global::Assets.Scripts.Boid.Boid> boids) {
            if (boids.Count > 0) {
                Vector3 averagePosition = getAveragePosition(boids);
                Vector3 aim = averagePosition - boid.Position;
                aim.Normalize();
                aim *= boid.Properties.Speed;
                Vector3 steeringDirection = aim - boid.Velocity;
                steeringDirection = Vector3.ClampMagnitude(steeringDirection, this.MaxForce);
                return steeringDirection;
            }
            return Vector3.zero;
        }

        private static Vector3 getAveragePosition(List<global::Assets.Scripts.Boid.Boid> boids) {
            Vector3 averagePosition = Vector3.zero;

            foreach (global::Assets.Scripts.Boid.Boid otherBoid in boids) {
                averagePosition += otherBoid.Position;
            }

            return averagePosition / boids.Count;
        }

        protected Vector3 Separation(List<global::Assets.Scripts.Boid.Boid> boids) {
            Vector3 steeringDirectionAggregator = Vector3.zero;
            int count = 0;
            foreach (global::Assets.Scripts.Boid.Boid otherBoid in boids) {
                count++;
                steeringDirectionAggregator += calculateSteeringDirection(otherBoid);
            }
            return calculateAverageSteeringDirection(steeringDirectionAggregator, count);
        }

        private Vector3 calculateSteeringDirection(global::Assets.Scripts.Boid.Boid otherBoid) {
            Vector3 steeringDirection = Vector3.zero;
            float distance = Vector3.Distance(boid.Position, otherBoid.Position);
            if (distance < boid.MinimumDistance) {
                Vector3 difference = boid.Position - otherBoid.Position;
                difference.Normalize();
                difference /= distance; //weight by distance
                steeringDirection += difference;
            }
            return steeringDirection;
        }

        private Vector3 calculateAverageSteeringDirection(Vector3 steeringDirectionAggregator, int count) {
            Vector3 averageSteeringDirection = steeringDirectionAggregator;
            if (count > 0) {
                averageSteeringDirection /= count;
            }
            if (averageSteeringDirection.magnitude > 0) {
                averageSteeringDirection.Normalize();
                averageSteeringDirection *= boid.Properties.Speed;
                averageSteeringDirection = Vector3.ClampMagnitude(steeringDirectionAggregator, MaxForce);
            }
            return averageSteeringDirection;
        }

        protected Vector3 Alignment(List<global::Assets.Scripts.Boid.Boid> boids) {
            if (boids.Count > 0) {
                Vector3 averageHeading = getAverageHeading(boids);
                averageHeading.Normalize();
                averageHeading *= boid.Properties.Speed;
                Vector3 steeringDirection = averageHeading - boid.Velocity;
                steeringDirection = Vector3.ClampMagnitude(steeringDirection, MaxForce);
                return steeringDirection;
            }
            return Vector3.zero;
        }

        private static Vector3 getAverageHeading(List<global::Assets.Scripts.Boid.Boid> boids) {
            Vector3 averageHeading = Vector3.zero;
            foreach (global::Assets.Scripts.Boid.Boid otherBoid in boids) {
                averageHeading += otherBoid.Velocity;
            }
            averageHeading /= boids.Count;
            return averageHeading;
        }

        protected Vector3 PlaneAvoidance() {
            Plane[] boundaries = environmentManager.CurrentEnvironment.Boundaries;
            Vector3 steeringDirection = Vector3.zero;
            foreach (Plane boundary in boundaries) {
                //if really close proximity to a plane boundary
                if (boundary.GetDistanceToPoint(boid.Position) < boid.MinimumDistance) {
                    Vector3 avoidDirection = boundary.normal;
                    avoidDirection *= boid.Properties.Speed;
                    avoidDirection = Vector3.ClampMagnitude(avoidDirection, MaxForce);
                    steeringDirection += avoidDirection;
                }

                //if directly facing boundary and within reasonable distance from plane boundary
                Ray direction = new Ray(boid.Position, boid.Velocity);
                float distance;
                if (boundary.Raycast(direction, out distance)) {
                    if (distance < 35) {
                        Vector3 avoidDirection = boundary.normal;
                        avoidDirection *= boid.Properties.Speed;
                        avoidDirection = Vector3.ClampMagnitude(avoidDirection, MaxForce);
                        steeringDirection += avoidDirection;
                    }
                }
            }
            return steeringDirection;
        }
    }
}
