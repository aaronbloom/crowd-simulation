using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Bar : Goal, ICollidable {

        public const string IdentifierStatic = "Bar/BarU";
        public static readonly Vector3 SizeStatic = new Vector3(4, 0, 4);
        public string PlacementPattern { get; set; }
        private static readonly Quaternion initialRotationOffSet = Quaternion.Euler(90, 0, 0);
        private static readonly Vector3 initialPositionOffSet = new Vector3(0, 0, 0);

        /// <summary>
        /// Creates new bar object
        /// </summary>
        public Bar() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = initialRotationOffSet;
            this.InitialPositionOffSet = initialPositionOffSet;
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

        /// <returns>This</returns>
        public WorldObject GetObject() {
            return this;
        }

        /// <summary>
        /// Checks the placement patter isn't the same as the current one
        /// </summary>
        /// <param name="pattern">new placement pattern</param>
        /// <returns>true if not the same</returns>
        public bool IsNewPlacementPattern(string pattern) {
            return pattern != PlacementPattern;
        }
    }
}
