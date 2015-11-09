using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour {

    private BoidBehaviour behaviour;
    private Vector3 _velocity;
    public Vector3  Velocity {
        get {
            return _velocity;
        }
        set {
            _velocity = value;
        }
    }
    private Vector3 acceleration;
    public float maxSpeed = 8.0f;
    public float maxForce = 0.05f;
    
	// Use this for initialization
	void Start () {
        this.Velocity = Random.onUnitSphere * Random.Range(maxSpeed / 2, maxSpeed);
        this.behaviour = new FlockingBehaviour(this, 10, 6);
	}

    // Update is called once per frame
    void Update () {

        this.acceleration += this.behaviour.updateAcceleration();

        this.Velocity += acceleration;
        this.Velocity = Vector3.ClampMagnitude(this.Velocity, maxSpeed);
        _velocity.y = 0;    //remove any tendency for the boid to want to move in the y axis (up/down)
        this.transform.position += (this.Velocity * Time.deltaTime);
        this.acceleration = Vector3.zero; //reset acceleration

        //Set boid to face direction of travel
        this.transform.rotation = Quaternion.LookRotation(this.Velocity);
    }
}
