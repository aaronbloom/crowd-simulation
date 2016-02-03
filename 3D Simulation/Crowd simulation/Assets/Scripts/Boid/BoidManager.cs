using System.Collections.Generic;
using Assets.Scripts.Analysis;
using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Boid {
    public class BoidManager {

        private const int boidHeight = 2;
        private const string BoidPrefab = "CylinderBoid";

        private readonly int NumberOfBoids;
        private readonly Quaternion rotation;
        private readonly EnvironmentManager EnvironmentManager;
        private readonly List<GameObject> boids;
        private readonly HeatMap heatMap;
        public static readonly float SpawningIntervalSeconds = 0.5f;
        public static readonly float HeatMapCaptureIntervalSeconds = 1f;

        public BoidManager(int numOfBoids) {
            EnvironmentManager = EnvironmentManager.Shared();
            NumberOfBoids = numOfBoids;
            rotation = Quaternion.identity;
            boids = new List<GameObject>(NumberOfBoids);
            heatMap = new HeatMap(boids);
        }

        public void AttemptBoidSpawn() {
            for (int i = 0; i < NumberOfBoids - boids.Count; i++) {
                spawnBoid();
            }
        }

        public void CaptureAnalysisData() {
            heatMap.Update();
        }

        public void DisplayHeatMap() {
            heatMap.Display();
        }

        private void spawnBoid() {
            if (EntranceAvaliable()) {
                Vector3 positionOffset = FindRandomEntrancePosition();
                Vector3 position = Vector3.zero + positionOffset;
                bool isOverLapping = false;
                foreach (var boid in boids) {
                    if (Vector3.Distance(position, boid.transform.position) < 2) {
                        isOverLapping = true;
                    }
                }
                if (!isOverLapping) {
                    boids.Add((GameObject) BootStrapper.Initialise(BoidPrefab, position, rotation));
                }
            }
        }

        private Vector3 generateRandomPosition() {
            Vector3 environmentBounds = EnvironmentManager.CurrentEnvironment.Bounds;
            int xLimit = (int)environmentBounds.x;
            int zLimit = (int)environmentBounds.z;

            return new Vector3(Random.Range(0, xLimit), boidHeight, Random.Range(0, zLimit));
        }

        private Vector3 FindRandomEntrancePosition() {
            List<Entrance> entrances = EnvironmentManager.CurrentEnvironment.World.Entrances;
            if (entrances.Count > 0) {
                Entrance entrance = entrances[Random.Range(0, entrances.Count)];
                float halfEntranceSize = entrance.Size/2;
                Vector3 internalOffSet = new Vector3(
                    Random.Range(-halfEntranceSize, halfEntranceSize),
                    boidHeight, //spawn on ground
                    Random.Range(-halfEntranceSize, halfEntranceSize));
                Vector3 position = entrance.GameObject.transform.position + internalOffSet;
                return position;
            }
            return Vector3.zero;
        }

        private bool EntranceAvaliable() {
            return EnvironmentManager.CurrentEnvironment.World.Entrances.Count > 0;
        }
    }
}
