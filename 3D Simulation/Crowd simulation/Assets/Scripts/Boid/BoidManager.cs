using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Analysis;
using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Boid {
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

        public BoidManager(int numberOfBoids, float genderBias) {
            environmentManager = EnvironmentManager.Shared();
            this.numberOfBoids = numberOfBoids;
            this.genderBias = genderBias;
            Boids = new List<Boid>(this.numberOfBoids);
            heatMap = new HeatMap(Boids);
        }

        public void AttemptBoidSpawn() {
            for (int i = 0; (i < numberOfBoids - Boids.Count) && (i < MaxSpawnRate); i++) {
                spawnBoid();
            }
        }

        public void CaptureAnalysisData() {
            heatMap.Update();
        }

        public void DisplayHeatMap() {
            heatMap.Display();
            BootStrapper.CameraManager.ActivateRTSCamera();
        }

        public void Update() {
            foreach (Boid boid in Boids) {
                boid.Update();
            }
        }

        public Boid FindBoid(GameObject gameObject) {
            return Boids.FirstOrDefault(boid => boid.HasGameObject(gameObject));
        }

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

        private bool entranceAvaliable() {
            return environmentManager.CurrentEnvironment.World.Entrances.Count > 0;
        }
    }
}
