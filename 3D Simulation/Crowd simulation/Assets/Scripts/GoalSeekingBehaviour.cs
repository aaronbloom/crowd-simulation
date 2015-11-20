using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GoalSeekingBehaviour : BoidBehaviour {

    public GoalSeekingBehaviour (Boid boid) {
        this.boid = boid;
    }
    private readonly Boid boid;
    private Node target;
    private Path path;

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
                if (Vector3.Distance(target.Position, this.boid.transform.position) < 5) {
                    int index = path.RemainingNodes.IndexOf(this.target) + 1;
                    if (index < path.RemainingNodes.Count) {
                        this.target = path.RemainingNodes[index];
                        if (this.target == null) return Vector3.zero;
                    }
                }
                Vector3 aim = this.target.Position - boid.transform.position;
                aim.Normalize();
                aim *= Boid.MaxSpeed;
                Vector3 steeringDirection = aim - boid.Velocity;
                steeringDirection = Vector3.ClampMagnitude(steeringDirection, Boid.MaxForce);
                return steeringDirection;
            }
        }
        return Vector3.zero;
    }

    public override Vector3 updateAcceleration() {
        Vector3 acceleration = Vector3.zero;
        acceleration += MoveAlongPath();
        return acceleration;
    }
}
