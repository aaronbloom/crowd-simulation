using UnityEngine;

public class BootStrapper : MonoBehaviour {

    private static readonly string PrefabFilepath = "Prefabs/";
    private static readonly string Camera = "Camera";

    //System Fields
    public static BoidManager BoidManager { get; private set; }
    public static EnvironmentManager EnvironmentManager { get; private set; }
    public static CameraController CameraController { get; private set; }

    void Awake() {
        EnvironmentManager = new EnvironmentManager();
    }

    void Start() {
        Initialise(Camera);
    }

    public static void StartSimulation(int numberOfBoids) {
        BoidManager = new BoidManager(numberOfBoids);
        BoidManager.SpawnBoids();
    }

    public static Object Initialise(string prefabName) {
        return MonoBehaviour.Instantiate(Resources.Load(PrefabFilepath + prefabName));
    }

    public static Object Initialise(string prefabName, Vector3 position, Quaternion rotation) {
        return MonoBehaviour.Instantiate(Resources.Load(PrefabFilepath + prefabName), position, rotation);
    }

}

