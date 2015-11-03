using UnityEngine;
using System.Collections;

public class BoidManager : MonoBehaviour {

    private EnvironmentManager EnvironmentManager;
    public int NumberOfBoids = 1;

    private void SpawnBoids () {
        System.Random random = new System.Random();

        for (int i = 0; i < NumberOfBoids; i++)
        {
            Vector3 positionOffset = new Vector3(
                random.Next(0, (int)EnvironmentManager.Bounds.x),
                random.Next(0, (int)EnvironmentManager.Bounds.y),
                random.Next(0, (int)EnvironmentManager.Bounds.z));
            Instantiate(Resources.Load("Prefabs/CylinderBoid"), transform.position + positionOffset, transform.rotation);
        }
    }

    // Use this for initialization
    void Start () {
        EnvironmentManager = FindObjectOfType(typeof(EnvironmentManager)) as EnvironmentManager;
        SpawnBoids();
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
