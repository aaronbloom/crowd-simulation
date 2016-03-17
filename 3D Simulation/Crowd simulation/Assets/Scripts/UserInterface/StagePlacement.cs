using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Environment.World;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
    internal class StagePlacement {
        private static readonly int left = 0;
        private static readonly int back = 90;
        private static readonly int right = 180;
        private static readonly int forward = 270;

        public static void RecalcStages() {
            List<Stage> stages = new List<Stage>(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Stages);
            foreach (Stage stage in stages) {
                int[] directionsBlocked = StagePlacement.directionsBlocked(stage);
                int sides = directionsBlocked.Sum();
                string pattern = string.Join("", directionsBlocked.Select(x => x.ToString()).ToArray());
                string barShape = "";
                int direction = -1;
                switch (sides) {
                    case 3:
                        barShape = "stage/stageMiddle";
                        direction = GetDirectionThreeSides(pattern);
                        break;
                    case 2:
                        barShape = "stage/stageCorner";
                        direction = GetDirectionTwoSides(pattern);
                      
                        break;
                    case 1:
                        barShape = "stage/stageFull";
                        direction = GetDirectionOneSide(pattern);
                        break;
                }
                if (direction != -1 && barShape != "") {
                    tryUpdatePattern(pattern, barShape, stage, direction);
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

        private static void tryUpdatePattern(string pattern, string prefab, Stage stage, int yVal) {
            if (stage.IsNewPlacementPattern(pattern)) {
                stage.placementPattern = pattern;
                stage.ChangePrefab(prefab);
                stage.GameObject.transform.rotation = rotateToY(stage, yVal);
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
    }
}
