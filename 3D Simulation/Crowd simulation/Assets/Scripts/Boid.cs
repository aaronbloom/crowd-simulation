using UnityEngine;
using System.Collections.Generic;

public class Boid : MonoBehaviour {

    private BoidBehaviour behaviour;
    public Vector3 velocity;
    private Vector3 acceleration;
    public float maxSpeed = 8.0f;
    public float maxForce = 0.05f;
    
	// Use this for initialization
	void Start () {
        this.velocity = Random.onUnitSphere * Random.Range(maxSpeed / 2, maxSpeed);
        this.behaviour = new FlockingBehaviour(this);
	}

    // Update is called once per frame
    void Update () {
        this.acceleration += this.behaviour.updateAcceleration();

        this.velocity += acceleration;
        this.velocity = Vector3.ClampMagnitude(this.velocity, maxSpeed);
        this.transform.position += (this.velocity * Time.deltaTime);
        this.acceleration = Vector3.zero; //reset acceleration

        //Set boid to face direction of travel
        this.transform.rotation = Quaternion.LookRotation(this.velocity);
    }
}
