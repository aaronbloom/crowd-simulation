using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Bar : Goal, Collidable {
        public const string IdentifierStatic = "Bar/BarU";
        public static Vector3 SizeStatic = new Vector3(4, 0, 4);
        public string placementPattern { get; set; }

        public bool IsNewPlacementPattern(string pattern)
        {
            return pattern != placementPattern;
        }

        public Bar() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = Quaternion.Euler(90, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, 0, 0);
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

        public WorldObject getObject() {
            return this;
        }
    }
}
