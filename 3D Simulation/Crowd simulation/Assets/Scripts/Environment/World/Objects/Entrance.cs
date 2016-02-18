using System.Linq.Expressions;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Entrance : WorldObject {
        public const string IdentifierStatic = "Entrance";
        public static Vector3 SizeStatic = new Vector3(4, 0, 4);

        public Entrance() : base() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = Quaternion.Euler(90, 0, 0);
            this.InitialPositionOffSet = new Vector3(0, 0.01f, 0);
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

    }
}
