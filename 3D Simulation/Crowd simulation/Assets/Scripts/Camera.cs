using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour {

    private EnvironmentManager environmentManager;
    private Vector3 rotationCenter;
    private float rotationSpeed;

    // Use this for initialization
    void Start () {
        this.environmentManager = EnvironmentManager.Shared();
        rotationCenter = (this.environmentManager.Bounds / 2) + this.environmentManager.transform.position;
        rotationSpeed = 20;
        transform.LookAt(rotationCenter);
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(rotationCenter, Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
