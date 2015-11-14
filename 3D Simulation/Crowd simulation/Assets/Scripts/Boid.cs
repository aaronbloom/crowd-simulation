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

    // Use this for initialization
    void Start () {
        this._velocity = Random.onUnitSphere * Random.Range(MaxSpeed / 2, MaxSpeed);
        this.behaviour = new FlockingBehaviour(this, 10, 6);
	}

    // Update is called once per frame
    void Update () {
        this.acceleration += this.behaviour.updateAcceleration();

        this._velocity += acceleration;
        this._velocity = Vector3.ClampMagnitude(this._velocity, MaxSpeed);
        this._velocity.y = 0; //remove any tendency for the boid to want to move in the y axis (up/down)
        this.transform.position += (this._velocity * Time.deltaTime);
        this.acceleration = Vector3.zero; //reset acceleration

        //Set boid to face direction of travel
        this.transform.rotation = Quaternion.LookRotation(this._velocity);
    }
}
