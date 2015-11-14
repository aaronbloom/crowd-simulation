using UnityEngine;
using System.Collections;

public class BoidManager : MonoBehaviour {

    public int NumberOfBoids = 1;

    private EnvironmentManager EnvironmentManager;
    private const int boidHeight = 2;

    private void SpawnBoids() {
        System.Random random = new System.Random();

        for (int i = 0; i < NumberOfBoids; i++) {
            Vector3 positionOffset = new Vector3(
                random.Next(0, (int)EnvironmentManager.Bounds.x),
                boidHeight,
                random.Next(0, (int)EnvironmentManager.Bounds.z));
            Instantiate(Resources.Load("Prefabs/CylinderBoid"), transform.position + positionOffset, transform.rotation);
        }
    }

    // Use this for initialization
    void Start() {
        EnvironmentManager = FindObjectOfType(typeof(EnvironmentManager)) as EnvironmentManager;
        SpawnBoids();
    }

    // Update is called once per frame
    void Update() {

    }
}
