using UnityEngine;

class BootStrapper : MonoBehaviour {

    private static readonly string PrefabFilepath = "Prefabs/";
    private static readonly string Camera = "Camera";

    //System Fields
    private BoidManager boidManager;
    private EnvironmentManager environmentManager;
    private CameraController cameraController;

    void Awake() {
        environmentManager = new EnvironmentManager();
    }

    void Start() {
        initialise(Camera);
    }

    public void StartSimulation(int numberOfBoids) {
        boidManager = new BoidManager(numberOfBoids);
        boidManager.SpawnBoids();
    }

    static void initialise(string prefabName) {
        MonoBehaviour.Instantiate(Resources.Load(PrefabFilepath + prefabName));
    }

    static void initialise(string prefabName, Vector3 position, Quaternion rotation) {
        MonoBehaviour.Instantiate(Resources.Load(PrefabFilepath + prefabName), position, rotation);
    }

}

