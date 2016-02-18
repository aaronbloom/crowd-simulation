using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Bar : Goal {
        public const string IdentifierStatic = "Bar";
        public static Vector3 SizeStatic = new Vector3(4, 0, 4);

        public Bar() : base() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = Quaternion.Euler(90, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, 0.01f, 0);
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

    }
}
