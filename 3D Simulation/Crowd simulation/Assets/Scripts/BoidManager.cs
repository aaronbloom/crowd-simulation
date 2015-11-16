using UnityEngine;
using System.Collections;

public class BoidManager {   
    
    private const int boidHeight = 2;
    private int NumberOfBoids;
    private Vector3 position;
    private Quaternion rotation;
    private EnvironmentManager EnvironmentManager;

    public BoidManager(int numOfBoids) {
        EnvironmentManager = EnvironmentManager.Shared();
        NumberOfBoids = numOfBoids;
        position = Vector3.zero;
        rotation = Quaternion.identity;
    }

    public void SpawnBoids() {
        System.Random random = new System.Random();

        for (int i = 0; i < NumberOfBoids; i++) {
            Vector3 positionOffset = new Vector3(
                random.Next(0, (int)EnvironmentManager.Bounds.x),
                boidHeight,
                random.Next(0, (int)EnvironmentManager.Bounds.z));
            MonoBehaviour.Instantiate(Resources.Load("Prefabs/CylinderBoid"), position + positionOffset, rotation);
        }
    }

}
