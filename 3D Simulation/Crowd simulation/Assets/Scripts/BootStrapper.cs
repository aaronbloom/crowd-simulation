using System.Collections;
using Assets.Scripts.Boid;
using Assets.Scripts.Camera;
using Assets.Scripts.Environment;
using UnityEngine;

namespace Assets.Scripts {
    public class BootStrapper : MonoBehaviour {

        private const string PrefabFilepath = "Prefabs/";
        private const string CaptureHeatMap = "boidHeatMap";
        private const string BoidSpawning = "boidSpawningTimer";

        //System Fields
        public static BoidManager BoidManager { get; private set; }
        public static EnvironmentManager EnvironmentManager { get; private set; }
        public static CameraManager CameraManager { get; private set; }
        public static bool Pause { get; private set; }

        void Awake() {
            Pause = false;
            EnvironmentManager = new EnvironmentManager();
            CameraManager = new CameraManager();
        }

        void Update() {
            if (BoidManager != null) {
                BoidManager.Update();
            }
        }

        void Start() {
            CameraManager.ActivateRTSCamera();
        }

        void OnDrawGizmos() {
            if (EnvironmentManager != null)
            EnvironmentManager.OnDrawGizmos();
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
            return Instantiate(Resources.Load(PrefabFilepath + prefabName));
        }

        public static Object Initialise(string prefabName, Vector3 position, Quaternion rotation) {
            return Instantiate(Resources.Load(PrefabFilepath + prefabName), position, rotation);
        }

        // ReSharper disable once UnusedMember.Local - Called with StartCoroutine above
        private IEnumerator boidSpawningTimer() {
            while (true) {
                yield return new WaitForSeconds(BoidManager.SpawningIntervalSeconds); //wait
                BoidManager.AttemptBoidSpawn();
            }
        }

        // ReSharper disable once UnusedMember.Local - Called with StartCoroutine above
        private IEnumerator boidHeatMap() {
            while (true) {
                BoidManager.CaptureAnalysisData();
                yield return new WaitForSeconds(BoidManager.HeatMapCaptureIntervalSeconds); //wait
            }
        }
    }
}

