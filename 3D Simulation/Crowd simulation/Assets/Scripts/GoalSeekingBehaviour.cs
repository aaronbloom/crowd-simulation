using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.Scripts.WorldObjects;
using Assets.Scripts;

public class GoalSeekingBehaviour : FlockingBehaviour {

    private Node target;
    private Path path;

    public GoalSeekingBehaviour (Boid boid, float viewingDistance, float minimumDistance) : base(boid, viewingDistance, minimumDistance) {
        this.boid = boid;
        this.MaxSpeed = 9.0f;
        this.MaxForce = 2.0f;
        this.VelocityDamping = 0.2f;
        this.SeparationFactor = 0.9f;
    }

    public void Seek(Node goal, Graph graph) {
        Node startNode = graph.FindClosestNode(boid.transform.position);
        path = Path.Navigate(graph, startNode, goal);
    }

    public void Seek(Goal goal, Graph graph) {
        Node startNode = graph.FindClosestNode(boid.transform.position);
        Node goalNode = graph.FindClosestNode(goal.GameObject.transform.position);
        path = Path.Navigate(graph, startNode, goalNode);
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
        List<Boid> boids = FindBoidsWithinView();

        Vector3 seperationDirection = Separation(boids);

        Vector3 acceleration = Vector3.zero;
        acceleration += seperationDirection * SeparationFactor;
        acceleration += MoveAlongPath();
        return acceleration;
    }

    public override void DrawGraphGizmo() {
        Gizmos.color = Color.green;
        path.DrawGraphGizmo();
    }

    private void TargetNextNodeAlongPath() {
        if (Vector3.Distance(target.Position, this.boid.transform.position) < 2) {
            int index = path.Nodes.IndexOf(this.target) + 1;
            if (index < path.Nodes.Count - 1) {
                this.target = path.Nodes[index];
            } else {
                //Goal Found
                if (UnityEngine.Random.Range(0, 10) < 0) {
                    switchBehaviourToLoiter();
                } else {
                    chooseNewGoal();
                }
            }
        }
    }

    public void chooseNewGoal() {
        //Do something more intelligent here.
        List<Goal> goals = BootStrapper.EnvironmentManager.CurrentEnvironment.World.Goals;
        if(goals.Count > 0) {
            Goal targetGoal = goals[(int) UnityEngine.Random.Range(0, goals.Count)];
            Seek(targetGoal, BootStrapper.EnvironmentManager.CurrentEnvironment.Graph);
        } else {
            switchBehaviourToLoiter();
        }
    }

    public void switchBehaviourToLoiter() {
        boid.behaviour = new LoiteringBehaviour(boid, path.Nodes[path.Nodes.Count - 2]);
    }

}
