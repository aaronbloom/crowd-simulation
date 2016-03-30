using System.Collections.Generic;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Analysis {

    /// <summary>
    /// Class to control the heat map analysis graphics and logic.
    /// </summary>
    public class HeatMap {

        private readonly List<Boid.Boid> boids;
        private readonly float[,] map;
        private readonly int width;
        private readonly int length;

        private bool displayed;

        /// <summary>
        /// Takes <typeparamref name="boids"/> and can reference them when updating the heatmap.
        /// </summary>
        /// <param name="boids">List of all simulated boids</param>
        public HeatMap(List<Boid.Boid> boids) {
            this.boids = boids;
            var bounds = BootStrapper.EnvironmentManager.CurrentEnvironment.Bounds;
            this.width = (int) (bounds.x/HeatMapTile.TileSize);
            this.length = (int) (bounds.z/HeatMapTile.TileSize);
            map = new float[width, length];
        }

        /// <summary>
        /// Tells the heat map to update once.
        /// </summary>
        public void Update() {
            foreach (var boid in boids) {
                this.addToMap(boid.Position);
            }
        }

        /// <summary>
        /// Generates, and initialises within unity the heatmap on the ground area of the environment.
        /// </summary>
        public void Display() {
            if (!displayed) {
                Debug.Log("Heat map display");
                displayed = true;
                var halfTileSize = HeatMapTile.TileSize/2;
                var displayMap = generateMap();
                for (int x = 0; x < width; x++) {
                    for (int z = 0; z < length; z++) {
                        Vector3 position = new Vector3((x*HeatMapTile.TileSize) + halfTileSize, 0,
                            (z*HeatMapTile.TileSize) + halfTileSize);
                        HeatMapTile heatMapTile = new HeatMapTile();
                        BootStrapper.EnvironmentManager.CurrentEnvironment.World.AddObject(
                            WorldObject.Initialise(heatMapTile, position, Vector3.zero));
                        heatMapTile.GameObject.GetComponent<Renderer>().material.color = new Color(displayMap[x, z], 0,
                            0, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Adds a single boids footfall for one time tick.
        /// </summary>
        /// <param name="position">Boids footfall position</param>
        private void addToMap(Vector3 position) {
            var x = (int)(position.x / HeatMapTile.TileSize);
            var z = (int)(position.z / HeatMapTile.TileSize);
            map[x, z]++;
        }

        /// <summary>
        /// Takes all the values, and averages them out into a consistent scale for the heat map.
        /// </summary>
        /// <returns>Scaled 2D array of the map, arranged in tiles</returns>
        private float[,] generateMap() {
            float[,] outputMap = new float[width, length];
            float maxValue = float.MinValue;
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++) {
                    if (map[x, z] > maxValue) {
                        maxValue = map[x, z];
                    }
                }
            }
            for (int x = 0; x < width; x++) {
                for (int z = 0; z < length; z++) {
                    outputMap[x, z] = (map[x, z] / maxValue);
                }
            }
            return outputMap;
        }


    }
}
