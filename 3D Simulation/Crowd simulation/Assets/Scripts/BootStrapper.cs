using System;
using System.Collections;
using Assets.Scripts.Boid;
using Assets.Scripts.Camera;
using Assets.Scripts.Environment;
using UnityEngine;
using UnityEngineInternal;
using Object = UnityEngine.Object;

namespace Assets.Scripts {
    public class BootStrapper : MonoBehaviour {

        private static readonly string PrefabFilepath = "Prefabs/";
        private static readonly string Camera = "Camera";

        //System Fields
        public static BoidManager BoidManager { get; private set; }
        public static EnvironmentManager EnvironmentManager { get; private set; }
        public static CameraController CameraController { get; private set; }
        public static bool Pause { get; private set; }
        public static bool CaptureHeatMap { get; private set; }

        void Awake() {
            Pause = false;
            EnvironmentManager = new EnvironmentManager();
        }

        void Start() {
            CameraController = ((GameObject) Initialise(Camera)).GetComponent<CameraController>();
        }

        public void StartSimulation(int numberOfBoids) {
            EnvironmentManager.CurrentEnvironment.Build();

            BoidManager = new BoidManager(numberOfBoids);
            StartCoroutine("BoidSpawningTimer");
            StartCoroutine("BoidHeatMap");
        }

        public void StopSimulation() {
            Pause = true;
            CaptureHeatMap = false;
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
            CaptureHeatMap = true;
            while (CaptureHeatMap)
            {
                BoidManager.CaptureAnalysisData();
                yield return new WaitForSeconds(BoidManager.HeatMapCaptureIntervalSeconds); //wait
            }
        }
    }
}

