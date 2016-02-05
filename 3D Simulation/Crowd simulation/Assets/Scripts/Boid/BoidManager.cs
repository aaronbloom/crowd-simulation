using System.Collections.Generic;
using Assets.Scripts.Analysis;
using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    public class BoidManager {

        private const int boidHeight = 2;
        private static readonly string[] MalePrefab = {"chr_mike", "chr_bro", "chr_beardo2"};
        private static readonly string[] FemalePrefab = {"chr_brookie", "chr_brookie", "chr_goth2"};

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
                    if (Vector3.Distance(position, boid.transform.position) < 3) {
                        isOverLapping = true;
                    }
                }
                if (!isOverLapping) {
                    BoidProperties boidProperties = new BoidProperties();
                    position.y = 0.1f;
                    Gender boidGender = boidProperties.Gender;
                    int index = Random.Range(0, 3);
                    string boidPrefab = boidGender == Gender.MALE ? MalePrefab[index] : FemalePrefab[index];
                    boids.Add((GameObject) BootStrapper.Initialise("mmmm/" + boidPrefab, position, rotation));
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
    }
}
