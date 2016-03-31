using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Environment.World;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.UserInterface {

    /// <summary>
    /// Calculates the rotation and shape of stages
    /// </summary>
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

        /// <summary>
        /// Recalculates stage positions
        /// </summary>
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

        /// <summary>
        /// <para>Returns an int[4] representing the four sides of <paramref name="obj"/></para>
        /// <para>Each number represents if that side is blocked by another WorldObject</para>
        /// <para>[0]: if 1, then left of <paramref name="obj"/> is blocked</para>
        /// <para>[1]: if 1, then right of <paramref name="obj"/> is blocked</para>
        /// <para>[2]: if 1, then up of <paramref name="obj"/> is blocked</para>
        /// <para>[3]: if 1, then down of <paramref name="obj"/> is blocked</para>
        /// </summary>
        /// <param name="obj">The WorldObject you wish to see has blocked sides</param>
        /// <returns>int[4] representing which directions are blocked by other WorldObjects</returns>
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

        /// <summary>
        /// Checks if a point (<paramref name="location"/>) is occupied by a WorldObject
        /// </summary>
        /// <param name="location">The point to check</param>
        /// <returns>1 if true, 0 if false</returns>
        private static int IsSpaceAlreadyOccupied(Vector3 location) {
            World world = BootStrapper.EnvironmentManager.CurrentEnvironment.World;
            return world.SpaceAlreadyOccupied(location) ? 1 : 0;
        }

        /// <summary>
        /// Tries to rotate to a new pattern, fails if the pattern is the same as the existing one
        /// </summary>
        /// <param name="pattern">The new pattern</param>
        /// <param name="prefab">The new prefab</param>
        /// <param name="stage">The Stage object to rotate</param>
        /// <param name="yVal">The new yAngle to rotate to</param>
        private static void tryUpdatePattern(string pattern, string prefab, Stage stage, int yVal) {
            if (stage.IsNewPlacementPattern(pattern)) {
                stage.PlacementPattern = pattern;
                stage.ChangePrefab(prefab);
                stage.GameObject.transform.rotation = rotateToY(stage, yVal);
            }
        }

        /// <summary>
        /// Gets the Quaternion result of rotating from <paramref name="obj"/>'s current rotation to the angle of <paramref name="yVal"/>
        /// </summary>
        /// <param name="obj">The Object to rotate</param>
        /// <param name="yVal">The angle to rotate to</param>
        /// <returns>The angle to rotate by</returns>
        private static Quaternion rotateToY(WorldObject obj, int yVal) {
            return Quaternion.Euler(obj.GameObject.transform.rotation.eulerAngles.x, yVal,
                obj.GameObject.transform.rotation.eulerAngles.z);
        }

        /// <summary>
        /// Calculates the required angle to rotate to from a pattern with 1 blocked side
        /// </summary>
        /// <param name="pattern">The pattern with 1 blocked side</param>
        /// <returns>The required angle</returns>
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

        /// <summary>
        /// Calculates the required angle to rotate to from a pattern with 2 blocked sides
        /// </summary>
        /// <param name="pattern">The pattern with 2 blocked sides</param>
        /// <returns>The required angle</returns>
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

        /// <summary>
        /// Calculates the required angle to rotate to from a pattern with 3 blocked sides
        /// </summary>
        /// <param name="pattern">The pattern with 3 blocked sides</param>
        /// <returns>The required angle</returns>
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
