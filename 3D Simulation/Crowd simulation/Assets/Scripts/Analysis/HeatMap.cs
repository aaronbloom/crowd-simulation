using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Analysis {
    public class HeatMap {
        private readonly List<GameObject> boids;
        private float[,] map;
        private int mappingCount;
        private static int resolution = 2;
        private readonly int width;
        private readonly int length;

        public HeatMap(List<GameObject> boids) {
            this.boids = boids;
            var bounds = BootStrapper.EnvironmentManager.CurrentEnvironment.Bounds;
            this.width = (int) (bounds.x/resolution);
            this.length = (int) (bounds.z/resolution);
            map = new float[width, length];
            mappingCount = 0;
        }

        private void addToMap(Vector3 position) {
            var x = (int) (position.x/resolution);
            var z = (int) (position.z/resolution);
            map[x, z]++;
        }

        public void Update() {
            mappingCount++;
            foreach (var boid in boids) {
                this.addToMap(boid.transform.position);
            }
        }

        public float[,] Map() {
            float[,] outputMap = {};
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++)
                {
                    outputMap[x, z] = (map[x,z] / mappingCount);
                }
            }
            return outputMap;
        }

    }
}
