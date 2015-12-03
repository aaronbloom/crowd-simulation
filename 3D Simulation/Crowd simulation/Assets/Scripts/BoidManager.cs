using UnityEngine;
using System.Collections;

public class BoidManager {

    private const int boidHeight = 2;
    private static readonly string BoidPrefab = "CylinderBoid";

    private int NumberOfBoids;
    private Quaternion rotation;
    private EnvironmentManager EnvironmentManager;

    public BoidManager(int numOfBoids) {
        EnvironmentManager = EnvironmentManager.Shared();
        NumberOfBoids = numOfBoids;
        rotation = Quaternion.identity;
    }

    public void SpawnBoids() {
        System.Random random = new System.Random();
        for (int i = 0; i < NumberOfBoids; i++) {
            spawnBoid(random);
        }
    }

    private void spawnBoid(System.Random random) {
        Vector3 positionOffset = generateRandomPosition(random);
        Vector3 position = Vector3.zero + positionOffset;
        BootStrapper.Initialise(BoidPrefab, position, rotation);
    }

    private Vector3 generateRandomPosition(System.Random random) {
        Vector3 environmentBounds = EnvironmentManager.CurrentEnvironment.Bounds;
        int xLimit = (int)environmentBounds.x;
        int zLimit = (int)environmentBounds.z;

        return new Vector3(random.Next(0, xLimit), boidHeight, random.Next(0, zLimit));
    }
}
