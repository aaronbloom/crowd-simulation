using UnityEngine;
using System.Collections.Generic;
using System.Linq.Expressions;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Analysis {
    public class HeatMap {
        private readonly List<Boid.Boid> boids;
        private readonly float[,] map;
        private readonly int width;
        private readonly int length;

        public HeatMap(List<Boid.Boid> boids) {
            this.boids = boids;
            var bounds = BootStrapper.EnvironmentManager.CurrentEnvironment.Bounds;
            this.width = (int) (bounds.x/HeatMapTile.TileSize);
            this.length = (int) (bounds.z/HeatMapTile.TileSize);
            map = new float[width, length];
        }

        private void addToMap(Vector3 position) {
            var x = (int) (position.x/ HeatMapTile.TileSize);
            var z = (int) (position.z/ HeatMapTile.TileSize);
            map[x, z]++;
        }

        public void Update() {
            foreach (var boid in boids) {
                this.addToMap(boid.Position);
            }
        }

        public float[,] Map() {
            float[,] outputMap = new float[width, length];
            float maxValue = float.MinValue;
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++) {
                    if (map[x, z] > maxValue) {
                        maxValue = map[x, z];
                    }
                }
            }
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    outputMap[x, z] = (map[x, z] / maxValue);
                }
            }
            return outputMap;
        }

        public void Display() {
            Debug.Log("Heat map display");
            var halfTileSize = HeatMapTile.TileSize/2;
            var displayMap = Map();
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++) {
                    Vector3 position = new Vector3((x*HeatMapTile.TileSize) + halfTileSize, 0, (z*HeatMapTile.TileSize) + halfTileSize);
                    HeatMapTile heatMapTile = new HeatMapTile();
                    BootStrapper.EnvironmentManager.CurrentEnvironment.World.AddObject(
                        WorldObject.Initialise(heatMapTile, position));
                    heatMapTile.GameObject.GetComponent<Renderer>().material.color = new Color(displayMap[x, z], 0, 0, 1);
                }
            }
        }

    }
}
