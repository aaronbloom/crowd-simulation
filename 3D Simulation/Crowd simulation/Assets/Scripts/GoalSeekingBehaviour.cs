using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GoalSeekingBehaviour : BoidBehaviour {

    private readonly Boid boid;
    private Node target;
    private Path path;

    public GoalSeekingBehaviour (Boid boid) {
        this.boid = boid;
        this.MaxSpeed = 8.0f;
        this.MaxForce = 0.5f;
        this.VelocityDamping = 0.5f;
    }

    public void Seek(Node goal, Graph graph) {
        Node startNode = graph.FindClosestNode(boid.transform.position);
        path = Path.Navigate(graph, startNode, goal);
    }

    private Vector3 MoveAlongPath() {
        if (path != null) {
            if (this.target == null) {
                this.target = path.FindClosestNode(boid.transform.position);
            }
            else {
                if (Vector3.Distance(target.Position, this.boid.transform.position) < 2) {
                    int index = path.RemainingNodes.IndexOf(this.target) + 1;
                    if (index < path.RemainingNodes.Count) {
                        this.target = path.RemainingNodes[index];
                        if (this.target == null) return Vector3.zero;
                    }
                }
                Vector3 aim = this.target.Position - boid.transform.position;
                aim.Normalize();
                aim *= MaxSpeed;
                Vector3 steeringDirection = aim - boid.Velocity;
                steeringDirection = Vector3.ClampMagnitude(steeringDirection, MaxForce);
                return steeringDirection;
            }
        }
        return Vector3.zero;
    }

    public override Vector3 InitialVelocity() {
        return Vector3.zero;
    }

    public override Vector3 updateAcceleration() {
        Vector3 acceleration = Vector3.zero;
        acceleration += MoveAlongPath();
        return acceleration;
    }

    public override void DrawGraphGizmo() {
        Gizmos.color = Color.green;
        path.DrawGraphGizmo();
    }
}
