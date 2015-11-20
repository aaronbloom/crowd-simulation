using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    private Vector3 _velocity;
    public Vector3 Velocity {
        get { return _velocity; }
    }

    private BoidBehaviour behaviour;
    private Vector3 acceleration;

    void Awake() {
        this.behaviour = new GoalSeekingBehaviour(this);
    }

    void Start() {
        this._velocity = this.behaviour.InitialVelocity();
        Graph graph = EnvironmentManager.Shared().CurrentEnvironment.graph;
        ((GoalSeekingBehaviour) this.behaviour).Seek(graph.FindClosestNode(Vector3.zero), graph);
    }

    void Update() {
        calculateNewPosition();
        resetAcceleration();
        faceTravelDirection();
    }

    void OnDrawGizmos() {
        this.behaviour.DrawGraphGizmo();
    }

    private void calculateNewPosition() {
        this.acceleration = calculateAcceleration(this.acceleration);
        this._velocity = calculateVelocity(this._velocity);
        this.transform.position += (this._velocity * Time.deltaTime);
    }

    private Vector3 calculateAcceleration(Vector3 acceleration) {
        return acceleration += this.behaviour.updateAcceleration();
    }

    private Vector3 calculateVelocity(Vector3 velocity) {
        velocity += acceleration;
        velocity = Vector3.ClampMagnitude(velocity, this.behaviour.MaxSpeed);
        velocity.y = 0;
        return velocity;
    }

    private void resetAcceleration() {
        this.acceleration = Vector3.zero;
    }

    private void faceTravelDirection() {
        this.transform.rotation = Quaternion.LookRotation(this._velocity);
    }
}
