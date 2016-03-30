using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Analysis;
using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Boid {

    /// <summary>
    /// The manager class for the boids
    /// </summary>
    public class BoidManager {

        public const float SpawningIntervalSeconds = 0.5f;
        public const float HeatMapCaptureIntervalSeconds = 1f;
        private const int MaxSpawnRate = 10;

        private const int BoidHeight = 2;

        public List<Boid> Boids { get; protected set; }

        private readonly int numberOfBoids;
        private readonly float genderBias;
        private readonly EnvironmentManager environmentManager;
        private readonly HeatMap heatMap;

        /// <summary>
        /// Constructor for boid manager
        /// </summary>
        /// <param name="numberOfBoids">Number of boids to manage</param>
        /// <param name="genderBias">Simulation gender bias constraint</param>
        public BoidManager(int numberOfBoids, float genderBias) {
            environmentManager = EnvironmentManager.Shared();
            this.numberOfBoids = numberOfBoids;
            this.genderBias = genderBias;
            Boids = new List<Boid>(this.numberOfBoids);
            heatMap = new HeatMap(Boids);
        }

        /// <summary>
        /// Tries to spawn a number of boids if possible within the environment.
        /// </summary>
        public void AttemptBoidSpawn() {
            for (int i = 0; (i < numberOfBoids - Boids.Count) && (i < MaxSpawnRate); i++) {
                spawnBoid();
            }
        }

        /// <summary>
        /// Statistics capture update
        /// </summary>
        public void CaptureAnalysisData() {
            heatMap.Update();
        }

        /// <summary>
        /// Display the heat map
        /// </summary>
        public void DisplayHeatMap() {
            heatMap.Display();
            BootStrapper.CameraManager.ActivateRTSCamera();
        }

        /// <summary>
        /// Update loop
        /// </summary>
        public void Update() {
            foreach (Boid boid in Boids) {
                boid.Update();
            }
        }
    
        /// <summary>
        /// Returns a boid based upon a GameObject its associated with.
        /// </summary>
        /// <param name="gameObject">GameObject</param>
        /// <returns>Associated boid</returns>
        public Boid FindBoid(GameObject gameObject) {
            return Boids.FirstOrDefault(boid => boid.HasGameObject(gameObject));
        }

        /// <summary>
        /// Spawn a boid within the environment (if possible)
        /// </summary>
        private void spawnBoid() {
            if (entranceAvaliable()) {
                Vector3 positionOffset = FindRandomEntrancePosition();
                Vector3 position = Vector3.zero + positionOffset;
                bool isOverLapping = false;
                foreach (Boid boid in Boids.Where(boid => Vector3.Distance(position, boid.Position) < 3)) {
                    isOverLapping = true;
                }
                if (!isOverLapping) {
                    Boids.Add(Boid.Spawn(position, genderBias));
                }
            }
        }

        /// <summary>
        /// Find the position of an avaliable entrance, that is not occupied.
        /// </summary>
        /// <returns></returns>
        private Vector3 FindRandomEntrancePosition() {
            List<Entrance> entrances = environmentManager.CurrentEnvironment.World.Entrances;
            if (entrances.Count > 0) {
                Entrance entrance = entrances[Random.Range(0, entrances.Count)];
                float spawnAreaX = entrance.Size.x / 3;
                float spawnAreaY = entrance.Size.z / 3;
                Vector3 internalOffSet = new Vector3(
                    Random.Range(-spawnAreaX, spawnAreaX),
                    BoidHeight, //spawn on ground
                    Random.Range(-spawnAreaY, spawnAreaY));
                Vector3 position = entrance.GameObject.transform.position + internalOffSet;
                return position;
            }
            return Vector3.zero;
        }

        /// <summary>
        /// Are there more than zero entrances
        /// </summary>
        /// <returns>Is there an entrance avaliable</returns>
        private bool entranceAvaliable() {
            return environmentManager.CurrentEnvironment.World.Entrances.Count > 0;
        }
    }
}
