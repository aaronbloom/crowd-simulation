using UnityEngine;

class Bootstrapper : MonoBehaviour {

    //System properties
    int boidPopulation = 100;

    //System Fields
    private BoidManager boidManager;
    private EnvironmentManager environmentManager;

    void Awake() {
        environmentManager = new EnvironmentManager();
    }

    void Start() {
        boidManager = new BoidManager(boidPopulation);
        boidManager.SpawnBoids();
    }

}

