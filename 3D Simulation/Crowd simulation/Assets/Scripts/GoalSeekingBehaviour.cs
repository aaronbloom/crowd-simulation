using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GoalSeekingBehaviour : BoidBehaviour {

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
                this.TargetNextNodeAlongPath();
                if (this.target == null) return Vector3.zero;
                return this.SteerTowardsPoint(this.target.Position);
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

    private void TargetNextNodeAlongPath() {
        if (Vector3.Distance(target.Position, this.boid.transform.position) < 2) {
            int index = path.RemainingNodes.IndexOf(this.target) + 1;
            if (index < path.RemainingNodes.Count) {
                this.target = path.RemainingNodes[index];
            }
        }
    }
}
