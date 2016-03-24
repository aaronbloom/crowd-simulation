using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Bar : Goal, ICollidable {

        public const string IdentifierStatic = "Bar/BarU";
        public static readonly Vector3 SizeStatic = new Vector3(4, 0, 4);
        public string PlacementPattern { get; set; }
        private static readonly Quaternion initialRotationOffSet = Quaternion.Euler(90, 0, 0);
        private static readonly Vector3 initialPositionOffSet = new Vector3(0, 0, 0);

        public Bar() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = initialRotationOffSet;
            this.InitialPositionOffSet = initialPositionOffSet;
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

        public WorldObject GetObject() {
            return this;
        }

        public bool IsNewPlacementPattern(string pattern) {
            return pattern != PlacementPattern;
        }
    }
}
