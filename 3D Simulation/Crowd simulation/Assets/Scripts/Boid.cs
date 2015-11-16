using UnityEngine;

public class Boid : MonoBehaviour {

    private Vector3 _velocity;
    public Vector3 Velocity {
        get { return _velocity; }
    }

    private BoidBehaviour behaviour;
    private Vector3 acceleration;
    private float visualRotationSpeed = 1f;
    public static readonly float MaxSpeed = 8.0f;
    public static readonly float MaxForce = 0.05f;

    void Start() {
        this._velocity = Random.onUnitSphere * Random.Range(MaxSpeed / 2, MaxSpeed);
        this.behaviour = new FlockingBehaviour(this, 10, 6);
    }

    void Update() {
        calculateNewPosition();
        resetAcceleration();
        faceTravelDirection();
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
        velocity = Vector3.ClampMagnitude(velocity, MaxSpeed);
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
