using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {

    private float viewingDistance = 20;
    public Vector3 velocity;
    private float maxSpeed = 0.1f;
    private float maxForce = 0.05f;
    
	// Use this for initialization
	void Start () {
        direction = Vector3.forward;
	}

    List<Boid> findBoidsWithinView()
    {
        GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");
        List<Boid> closeBoids = new List<Boid>();

        foreach(GameObject boid in boids)
        {
            float distance = Vector3.Distance(boid.transform.position, this.transform.position);
            if (distance < this.viewingDistance && distance != 0)
            {
                closeBoids.Add(boid.GetComponent<Boid>());
            }
        }

        return closeBoids;
    }


    Vector3 Alignment (List<Boid> boids)
    {
        Vector3 averageHeading = Vector3.zero;
        foreach (Boid boid in boids)
        {
            averageHeading += boid.velocity;
        }
        if (boids.Count > 0)
        {
            averageHeading /= boids.Count;
            averageHeading.Normalize();
            averageHeading *= this.maxSpeed;
            Vector3 steeringDirection = averageHeading - this.velocity;
            steeringDirection = Vector3.ClampMagnitude(steeringDirection, this.maxForce);
            return steeringDirection;
        }
        return Vector3.zero;
    }

    void Update () {
        float step = speed * Time.deltaTime;
        transform.position += this.direction * speed;
    }
}
