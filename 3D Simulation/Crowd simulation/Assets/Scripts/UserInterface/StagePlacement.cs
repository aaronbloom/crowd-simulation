using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Environment.World;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
    internal class StagePlacement {

        private const string StageMiddle = "stage/stageMiddle";
        private const string StageCorner = "stage/stageCorner";
        private const string StageFull = "stage/StageFull";
        private const string Empty = "";

        private const int Left = 0;
        private const int Back = 90;
        private const int Right = 180;
        private const int Forward = 270;
        private const float WhiskerDepth = 0.1f;

        public static void RecalcStages() {
            List<Stage> stages = new List<Stage>(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Stages);
            foreach (Stage stage in stages) {
                int[] directionsBlocked = StagePlacement.directionsBlocked(stage);
                int sides = directionsBlocked.Sum();
                string pattern = string.Join(Empty, directionsBlocked.Select(x => x.ToString()).ToArray());
                string stageShape = Empty;
                int direction = -1;
                switch (sides) {
                    case 3:
                        stageShape = StageMiddle;
                        direction = GetDirectionThreeSides(pattern);
                        break;
                    case 2:
                        stageShape = StageCorner;
                        direction = GetDirectionTwoSides(pattern);

                        break;
                    case 1:
                        stageShape = StageFull;
                        direction = GetDirectionOneSide(pattern);
                        break;
                }
                if ((direction != -1) && (stageShape != Empty)) {
                    tryUpdatePattern(pattern, stageShape, stage, direction);
                }
            }
        }

        private static int[] directionsBlocked(WorldObject obj) {
            Vector3 position = obj.GameObject.transform.position;
            Vector3 offsetX = new Vector3(obj.Size.x/2 + WhiskerDepth, 0, 0);
            Vector3 offsetZ = new Vector3(0, 0, obj.Size.z/2 + WhiskerDepth);

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
                stage.PlacementPattern = pattern;
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
                    direction = Back;
                    break;
                case "0100":
                    direction = Forward;
                    break;
                case "0010":
                    direction = Left;
                    break;
                case "0001":
                    direction = Right;
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
                    direction = Forward;
                    break;
                case "1100":
                    // =
                    break;
                case "1010":
                    direction = Left;
                    break;
                case "0101":
                    direction = Right;
                    break;
                case "1001":
                    direction = Back;
                    break;
            }
            return direction;
        }

        private static int GetDirectionThreeSides(string pattern) {
            int direction = -1;
            switch (pattern) {
                case "0111":
                    direction = Forward;
                    break;
                case "1011":
                    direction = Back;
                    break;
                case "1101":
                    direction = Right;
                    break;
                case "1110":
                    direction = Left;
                    break;
            }
            return direction;
        }
    }
}
