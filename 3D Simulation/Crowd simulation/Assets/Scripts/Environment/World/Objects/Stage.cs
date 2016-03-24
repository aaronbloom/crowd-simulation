using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Stage : Goal, ICollidable {

        public const string IdentifierStatic = "Stage/stageFull";
        public static readonly Vector3 SizeStatic = new Vector3(4, 2, 4);
        private static readonly Quaternion initialRotationOffSet = Quaternion.Euler(0, 0, 0);
        private static readonly Vector3 initialPositionOffSet = new Vector3(0, -1, 0);
        public string PlacementPattern { get; set; }

        public Stage() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = initialRotationOffSet;
            this.InitialPositionOffSet = initialPositionOffSet;
            this.FrontPadding = 2;
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
