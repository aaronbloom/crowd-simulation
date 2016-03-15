using System.Collections.Generic;
using Assets.Scripts.Analysis;
using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Boid {
    public class BoidManager {

        private const int boidHeight = 2;

        private readonly int NumberOfBoids;
        private readonly float _genderBias;
        private readonly EnvironmentManager EnvironmentManager;
        public List<Boid> Boids { get; protected set; }
        private readonly HeatMap heatMap;
        public static readonly float SpawningIntervalSeconds = 0.5f;
        public static readonly float HeatMapCaptureIntervalSeconds = 1f;

        public BoidManager(int numOfBoids, float genderBias) {
            EnvironmentManager = EnvironmentManager.Shared();
            NumberOfBoids = numOfBoids;
            _genderBias = genderBias;
            Boids = new List<Boid>(NumberOfBoids);
            heatMap = new HeatMap(Boids);
        }

        public void AttemptBoidSpawn() {
            for (int i = 0; i < NumberOfBoids - Boids.Count; i++) {
                spawnBoid();
            }
        }

        public void CaptureAnalysisData() {
            heatMap.Update();
        }

        public void DisplayHeatMap() {
            heatMap.Display();
            BootStrapper.CameraManager.SwitchToRTSCamera();
        }

        public void Update() {
            foreach (Boid boid in Boids) {
                boid.Update();
            }
        }

        private void spawnBoid() {
            if (EntranceAvaliable()) {
                Vector3 positionOffset = FindRandomEntrancePosition();
                Vector3 position = Vector3.zero + positionOffset;
                bool isOverLapping = false;
                foreach (Boid boid in Boids) {
                    if (Vector3.Distance(position, boid.Position) < 3) {
                        isOverLapping = true;
                    }
                }
                if (!isOverLapping) {
                    Boids.Add(Boid.Spawn(position, _genderBias));
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
                float halfEntranceX = entrance.Size.x / 2;
                float halfEntranceY = entrance.Size.z / 2;
                Vector3 internalOffSet = new Vector3(
                    Random.Range(-halfEntranceX, halfEntranceX),
                    boidHeight, //spawn on ground
                    Random.Range(-halfEntranceY, halfEntranceY));
                Vector3 position = entrance.GameObject.transform.position + internalOffSet;
                return position;
            }
            return Vector3.zero;
        }

        private bool EntranceAvaliable() {
            return EnvironmentManager.CurrentEnvironment.World.Entrances.Count > 0;
        }

        public Boid FindBoid(GameObject gameObject) {
            foreach (Boid boid in Boids) {
                if (boid.HasGameObject(gameObject)) {
                    return boid;
                }
            }
            return null;
        }
    }
}
