using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Entrance : WorldObject {

        public const string IdentifierStatic = "Door";
        public static readonly Vector3 SizeStatic = new Vector3(4, 0, 4);
        private static readonly Quaternion initialRotationOffSet = Quaternion.Euler(90, 180, 0);
        private static readonly Vector3 initialPositionOffSet = new Vector3(0, 0, 0);

        public Entrance() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = initialRotationOffSet;
            this.InitialPositionOffSet = initialPositionOffSet;
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

    }
}
