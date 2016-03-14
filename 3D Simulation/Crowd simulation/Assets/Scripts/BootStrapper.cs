using System.Collections;
using Assets.Scripts.Boid;
using Assets.Scripts.Environment;
using RTS_Cam;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts {
    public class BootStrapper : MonoBehaviour {

        private static readonly string PrefabFilepath = "Prefabs/";
        private static readonly string CameraPrefab = "RTS_Camera";

        //System Fields
        public static BoidManager BoidManager { get; private set; }
        public static EnvironmentManager EnvironmentManager { get; private set; }
        public static RTS_Camera CameraController { get; private set; }
        public static bool Pause { get; private set; }
        private const string CaptureHeatMap = "BoidHeatMap";
        private const string BoidSpawning = "BoidSpawningTimer";

        void Awake() {
            Pause = false;
            EnvironmentManager = new EnvironmentManager();
        }

        void Start() {
            CameraController = GameObject.Find(CameraPrefab).GetComponent(typeof (RTS_Camera)) as RTS_Camera;
        }

        void Update() {
            if (BoidManager != null) {
                BoidManager.Update();
            }
        }

        public void StartSimulation(int numberOfBoids, float genderBias) {
            EnvironmentManager.CurrentEnvironment.Build();

            BoidManager = new BoidManager(numberOfBoids, genderBias);
            StartCoroutine(CaptureHeatMap);
            StartCoroutine(BoidSpawning);
        }

        public void StopSimulation() {
            Pause = true;
            StopCoroutine(CaptureHeatMap);
            StopCoroutine(BoidSpawning);
        }

        public static Object Initialise(string prefabName) {
            return MonoBehaviour.Instantiate(Resources.Load(PrefabFilepath + prefabName));
        }

        public static Object Initialise(string prefabName, Vector3 position, Quaternion rotation) {
            return MonoBehaviour.Instantiate(Resources.Load(PrefabFilepath + prefabName), position, rotation);
        }

        private IEnumerator BoidSpawningTimer() {
            while (true) {
                yield return new WaitForSeconds(BoidManager.SpawningIntervalSeconds); //wait
                BoidManager.AttemptBoidSpawn();
            }
        }

        private IEnumerator BoidHeatMap() {
            while (true) {
                BoidManager.CaptureAnalysisData();
                yield return new WaitForSeconds(BoidManager.HeatMapCaptureIntervalSeconds); //wait
            }
        }
    }
}

