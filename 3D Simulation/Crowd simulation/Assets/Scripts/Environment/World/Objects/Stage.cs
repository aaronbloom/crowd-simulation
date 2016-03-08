using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Stage : Goal, Collidable {
        public const string IdentifierStatic = "Stage";
        public static Vector3 SizeStatic = new Vector3(4, 2, 4);

        public Stage() : base() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = Quaternion.Euler(0, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, 0, 0);
            this.Padding = new Vector3(4, 0, 0);
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

        public WorldObject getObject() {
            return this;
        }
    }
}
