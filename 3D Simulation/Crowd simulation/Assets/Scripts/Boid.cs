using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {
    private static float speed = 4;
    public Vector3 direction;
    private float viewingDistance = 20;
    
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

    void Update () {
        float step = speed * Time.deltaTime;
        transform.position += this.direction * speed;
    }
}
