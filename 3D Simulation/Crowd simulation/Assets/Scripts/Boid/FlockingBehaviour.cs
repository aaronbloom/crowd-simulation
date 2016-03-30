using System;
using System.Collections.Generic;
using Assets.Scripts.Environment;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    public class FlockingBehaviour : BoidBehaviour {

        private const int CloseBoidsCacheTime = 25;

        protected float SeparationFactor;

        private readonly EnvironmentManager environmentManager;
        private int closeBoidsCacheCurrent = CloseBoidsCacheTime;
        private List<Boid> closeBoids;

        public FlockingBehaviour(Boid boid) {
            this.MaxForce = 0.05f;
            this.VelocityDamping = 1f;
            this.SeparationFactor = 1.5f;
            this.Boid = boid;
            this.environmentManager = EnvironmentManager.Shared();
        }

        public override Vector3 InitialVelocity() {
            float maxSpeed = Boid.Properties.MoveSpeed;
            return Random.onUnitSphere * Random.Range(maxSpeed / 2, maxSpeed);
        }

        public override Vector3 UpdateAcceleration() {
            FindBoidsWithinView();

            Vector3 cohesionDirection = Cohesion(closeBoids);
            Vector3 seperationDirection = Separation(closeBoids);
            Vector3 alignmentDirection = Alignment(closeBoids);
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

        protected List<Boid> FindBoidsWithinView() {
            closeBoidsCacheCurrent++;
            if (closeBoidsCacheCurrent >= CloseBoidsCacheTime) {
                closeBoidsCacheCurrent = 0;
                closeBoids = new List<Boid>();
                foreach (Boid otherBoid in BootStrapper.BoidManager.Boids) {
                    if (!ReferenceEquals(this.Boid, otherBoid) && isWithinView(otherBoid)) {
                        closeBoids.Add(otherBoid);
                    }
                }
            }
            return closeBoids;
        }

        private bool isWithinView(Boid otherBoid) {
            Vector3 boidPosition = Boid.Position;
            Vector3 otherBoidPosition = otherBoid.Position;
            float distance = Vector3.Distance(boidPosition, otherBoidPosition);
            return (distance < Boid.ViewingDistance) && distance != 0;
        }

        protected Vector3 Cohesion(List<Boid> boids) {
            if (boids.Count > 0) {
                Vector3 averagePosition = getAveragePosition(boids);
                Vector3 aim = averagePosition - Boid.Position;
                aim.Normalize();
                aim *= Boid.Properties.MoveSpeed;
                Vector3 steeringDirection = aim - Boid.Velocity;
                steeringDirection = Vector3.ClampMagnitude(steeringDirection, this.MaxForce);
                return steeringDirection;
            }
            return Vector3.zero;
        }

        private static Vector3 getAveragePosition(List<Boid> boids) {
            Vector3 averagePosition = Vector3.zero;

            foreach (Boid otherBoid in boids) {
                averagePosition += otherBoid.Position;
            }

            return averagePosition / boids.Count;
        }

        protected Vector3 Separation(List<Boid> boids) {
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
            float distance = Vector3.Distance(Boid.Position, otherBoid.Position);
            if (distance < Boid.MinimumDistance) {
                Vector3 difference = Boid.Position - otherBoid.Position;
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
                averageSteeringDirection = Vector3.ClampMagnitude(steeringDirectionAggregator, MaxForce);
            }
            return averageSteeringDirection;
        }

        protected Vector3 Alignment(List<Boid> boids) {
            if (boids.Count > 0) {
                Vector3 averageHeading = getAverageHeading(boids);
                averageHeading.Normalize();
                averageHeading *= Boid.Properties.MoveSpeed;
                Vector3 steeringDirection = averageHeading - Boid.Velocity;
                steeringDirection = Vector3.ClampMagnitude(steeringDirection, MaxForce);
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

        protected Vector3 PlaneAvoidance() {
            Plane[] boundaries = environmentManager.CurrentEnvironment.Boundaries;
            Vector3 steeringDirection = Vector3.zero;
            foreach (Plane boundary in boundaries) {
                //if really close proximity to a plane boundary
                if (boundary.GetDistanceToPoint(Boid.Position) < Boid.MinimumDistance) {
                    Vector3 avoidDirection = boundary.normal;
                    avoidDirection *= Boid.Properties.MoveSpeed;
                    avoidDirection = Vector3.ClampMagnitude(avoidDirection, MaxForce);
                    steeringDirection += avoidDirection;
                }

                //if directly facing boundary and within reasonable distance from plane boundary
                Ray direction = new Ray(Boid.Position, Boid.Velocity);
                float distance;
                if (boundary.Raycast(direction, out distance) && distance < 35) {
                    Vector3 avoidDirection = boundary.normal;
                    avoidDirection *= Boid.Properties.MoveSpeed;
                    avoidDirection = Vector3.ClampMagnitude(avoidDirection, MaxForce);
                    steeringDirection += avoidDirection;
                }
            }
            return steeringDirection;
        }
    }
}
