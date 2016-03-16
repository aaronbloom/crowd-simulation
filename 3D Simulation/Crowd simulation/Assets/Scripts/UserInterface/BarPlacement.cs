using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
    class BarPlacement {
        private static int left = 0;
        private static int back = 90;
        private static int right = 180;
        static int forward = 270;

        public static void RecalcBars() {
            List<Bar> bars = new List<Bar>(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Bars);
            foreach (Bar bar in bars) {
                int[] directionsBlocked = BarPlacement.directionsBlocked(bar);
                int sides = directionsBlocked.Sum();
                string pattern = string.Join("", directionsBlocked.Select(x => x.ToString()).ToArray());

                switch (sides) {
                    case 4:
                        switch (pattern) {
                            case "1111":
                                switch (bar.placementPattern) {
                                    case "0111":
                                        tryUpdatePattern(pattern, "bar/bar¬", bar, right);
                                        break;
                                    case "1011":
                                        tryUpdatePattern(pattern, "bar/bar¬", bar, left);
                                        break;
                                    case "1101":
                                        tryUpdatePattern(pattern, "bar/bar¬", bar, right);
                                        break;
                                    case "1110":
                                        tryUpdatePattern(pattern, "bar/bar¬", bar, forward);
                                        break;
                                }
                                break;
                        }
                        break;
                    case 3:
                        switch (pattern) {
                            case "0111":
                                tryUpdatePattern(pattern, "bar/barI", bar, forward);
                                break;
                            case "1011":
                                tryUpdatePattern(pattern, "bar/barI", bar, back);
                                break;
                            case "1101":
                                tryUpdatePattern(pattern, "bar/barI", bar, right);
                                break;
                            case "1110":
                                tryUpdatePattern(pattern, "bar/barI", bar, left);
                                break;
                        }
                        break;
                    case 2:
                        switch (pattern) {
                            case "0011":
                                // =
                                break;
                            case "0110":
                                tryUpdatePattern(pattern, "bar/barL", bar, forward);
                                break;
                            case "1100":
                                // =
                                break;
                            case "1010":
                                tryUpdatePattern(pattern, "bar/barL", bar, left);
                                break;
                            case "0101":
                                tryUpdatePattern(pattern, "bar/barL", bar, right);
                                break;
                            case "1001":
                                tryUpdatePattern(pattern, "bar/barL", bar, back);
                                break;
                        }
                        break;
                    case 1:
                        switch (pattern) {
                            case "1000":
                                tryUpdatePattern(pattern, "bar/barU", bar, back);
                                break;
                            case "0100":
                                tryUpdatePattern(pattern, "bar/barU", bar, forward);
                                break;
                            case "0010":
                                tryUpdatePattern(pattern, "bar/barU", bar, left);
                                break;
                            case "0001":
                                tryUpdatePattern(pattern, "bar/barU", bar, right);
                                break;
                        }
                        break;
                }
            }
        }

        private static int[] directionsBlocked(WorldObject obj) {
            float whiskerDepth = 0.1f;
            Vector3 position = obj.GameObject.transform.position;
            Vector3 offsetX = new Vector3((obj.Size.x / 2) + whiskerDepth, 0, 0);
            Vector3 offsetZ = new Vector3(0, 0, (obj.Size.z / 2) + whiskerDepth);

            int[] directionsBlocked = new int[4];
            directionsBlocked[0] = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(position - offsetX) ? 1 : 0;
            directionsBlocked[1] = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(position + offsetX) ? 1 : 0;
            directionsBlocked[2] = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(position - offsetZ) ? 1 : 0;
            directionsBlocked[3] = BootStrapper.EnvironmentManager.CurrentEnvironment.World.SpaceAlreadyOccupied(position + offsetZ) ? 1 : 0;

            return directionsBlocked;
        }

        private static void tryUpdatePattern(string pattern, string prefab, Bar bar, int yVal) {
            if (bar.IsNewPlacementPattern(pattern)) {
                bar.placementPattern = pattern;
                bar.ChangePrefab(prefab);
                bar.GameObject.transform.rotation = rotateToY(bar, yVal);
            }
        }

        private static Quaternion rotateToY(WorldObject obj, int yVal) {
            return Quaternion.Euler(obj.GameObject.transform.rotation.eulerAngles.x, yVal, obj.GameObject.transform.rotation.eulerAngles.z);
        }
    }
}
