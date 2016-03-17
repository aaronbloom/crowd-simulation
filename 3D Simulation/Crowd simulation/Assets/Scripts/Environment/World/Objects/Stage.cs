using System.Linq.Expressions;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Stage : Goal, Collidable {
        public const string IdentifierStatic = "Stage/stageFull";
        public static Vector3 SizeStatic = new Vector3(4, 2, 4);
        public string placementPattern { get; set; }

        public bool IsNewPlacementPattern(string pattern) {
            return pattern != placementPattern;
        }

        public Stage() : base() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = Quaternion.Euler(0, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, -1, 0);
            this.FrontPadding = 4;
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

        public WorldObject getObject() {
            return this;
        }
    }
}
