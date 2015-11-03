using UnityEngine;
using System.Collections;

public class Boid : MonoBehaviour {
    private static float speed = 4;
    public Vector3 direction;
    
	// Use this for initialization
	void Start () {
        direction = Vector3.forward;
	}
    void Update () {
        float step = speed * Time.deltaTime;
        transform.position += this.direction * speed;
    }
}
