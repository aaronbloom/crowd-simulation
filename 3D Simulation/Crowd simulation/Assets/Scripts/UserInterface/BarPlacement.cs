﻿using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Environment.World;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
    internal class BarPlacement {
        private static readonly int left = 0;
        private static readonly int back = 90;
        private static readonly int right = 180;
        private static readonly int forward = 270;

        public static void RecalcBars() {
            List<Bar> bars = new List<Bar>(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Bars);
            foreach (Bar bar in bars) {
                int[] directionsBlocked = BarPlacement.directionsBlocked(bar);
                int sides = directionsBlocked.Sum();
                string pattern = string.Join("", directionsBlocked.Select(x => x.ToString()).ToArray());
                string barShape = "";
                int direction = -1;
                switch (sides) {
                    case 4:
                        barShape = "bar/bar¬";
                        direction = GetDirectionFourSides(bar.placementPattern);
                        break;
                    case 3:
                        barShape = "bar/barI";
                        direction = GetDirectionThreeSides(pattern);
                        break;
                    case 2:
                        barShape = "bar/barL";
                        direction = GetDirectionTwoSides(pattern);
                        break;
                    case 1:
                        barShape = "bar/barU";
                        direction = GetDirectionOneSide(pattern);
                        break;
                }
                if (direction != -1 && barShape != "") {
                    tryUpdatePattern(pattern, barShape, bar, direction);
                }
            }
        }

        private static int[] directionsBlocked(WorldObject obj) {
            float whiskerDepth = 0.1f;
            Vector3 position = obj.GameObject.transform.position;
            Vector3 offsetX = new Vector3(obj.Size.x/2 + whiskerDepth, 0, 0);
            Vector3 offsetZ = new Vector3(0, 0, obj.Size.z/2 + whiskerDepth);

            int[] directionsBlocked = new int[4];
            directionsBlocked[0] = IsSpaceAlreadyOccupied(position - offsetX);
            directionsBlocked[1] = IsSpaceAlreadyOccupied(position + offsetX);
            directionsBlocked[2] = IsSpaceAlreadyOccupied(position - offsetZ);
            directionsBlocked[3] = IsSpaceAlreadyOccupied(position + offsetZ);

            return directionsBlocked;
        }

        private static int IsSpaceAlreadyOccupied(Vector3 location) {
            World world = BootStrapper.EnvironmentManager.CurrentEnvironment.World;
            return world.SpaceAlreadyOccupied(location) ? 1 : 0;
        }

        private static void tryUpdatePattern(string pattern, string prefab, Bar bar, int yVal) {
            if (bar.IsNewPlacementPattern(pattern)) {
                bar.placementPattern = pattern;
                bar.ChangePrefab(prefab);
                bar.GameObject.transform.rotation = rotateToY(bar, yVal);
            }
        }

        private static Quaternion rotateToY(WorldObject obj, int yVal) {
            return Quaternion.Euler(obj.GameObject.transform.rotation.eulerAngles.x, yVal,
                obj.GameObject.transform.rotation.eulerAngles.z);
        }

        private static int GetDirectionOneSide(string pattern) {
            int direction = -1;
            switch (pattern) {
                case "1000":
                    direction = back;
                    break;
                case "0100":
                    direction = forward;
                    break;
                case "0010":
                    direction = left;
                    break;
                case "0001":
                    direction = right;
                    break;
            }
            return direction;
        }

        private static int GetDirectionTwoSides(string pattern) {
            int direction = -1;
            switch (pattern) {
                case "0011":
                    // =
                    break;
                case "0110":
                    direction = forward;
                    break;
                case "1100":
                    // =
                    break;
                case "1010":
                    direction = left;
                    break;
                case "0101":
                    direction = right;
                    break;
                case "1001":
                    direction = back;
                    break;
            }
            return direction;
        }

        private static int GetDirectionThreeSides(string pattern) {
            int direction = -1;
            switch (pattern) {
                case "0111":
                    direction = forward;
                    break;
                case "1011":
                    direction = back;
                    break;
                case "1101":
                    direction = right;
                    break;
                case "1110":
                    direction = left;
                    break;
            }
            return direction;
        }

        private static int GetDirectionFourSides(string previousPattern) {
            int direction = -1;
            switch (previousPattern) {
                case "0111":
                    direction = right;
                    break;
                case "1011":
                    direction = left;
                    break;
                case "1101":
                    direction = right;
                    break;
                case "1110":
                    direction = forward;
                    break;
            }
            return direction;
        }
    }
}