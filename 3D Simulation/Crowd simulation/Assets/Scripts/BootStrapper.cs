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

        /// <summary>
        /// Intialises Bootstrapper
        /// </summary>
        void Awake() {
            Pause = false;
            EnvironmentManager = new EnvironmentManager();
            CameraManager = new CameraManager();
        }

        /// <summary>
        /// updates bootstrapper
        /// </summary>
        void Update() {
            if (BoidManager != null) {
                BoidManager.Update();
            }
        }

        /// <summary>
        /// Activates camera
        /// </summary>
        void Start() {
            CameraManager.ActivateRTSCamera();
        }

        /// <summary>
        /// Draws graph debugging gizmos
        /// </summary>
        void OnDrawGizmos() {
            if (EnvironmentManager != null)
            EnvironmentManager.OnDrawGizmos();
        }

        /// <summary>
        /// Starts the simulation with <paramref name="numberOfBoids"/> and <paramref name="genderBias"/>
        /// </summary>
        /// <param name="numberOfBoids">the number of boids</param>
        /// <param name="genderBias">the ratio of genders</param>
        public void StartSimulation(int numberOfBoids, float genderBias) {
            EnvironmentManager.CurrentEnvironment.Build();

            BoidManager = new BoidManager(numberOfBoids, genderBias);
            StartCoroutine(CaptureHeatMap);
            StartCoroutine(BoidSpawning);
        }

        /// <summary>
        /// Stops the simulation
        /// </summary>
        public void StopSimulation() {
            Pause = true;
            StopCoroutine(CaptureHeatMap);
            StopCoroutine(BoidSpawning);
        }

        /// <summary>
        /// Initialises a prefab to a gameobject
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns>the gameobject</returns>
        public static Object Initialise(string prefabName) {
            return Instantiate(Resources.Load(PrefabFilepath + prefabName));
        }

        /// <summary>
        /// intialises a prefab <paramref name="prefabName"/> to a gameobject at position <paramref name="position"/> with rotation <paramref name="rotation"/>
        /// </summary>
        /// <param name="prefabName">the prefab to initalise</param>
        /// <param name="position">the position to initalise at</param>
        /// <param name="rotation">the rotation of the prefab</param>
        /// <returns>the gameobject</returns>
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

