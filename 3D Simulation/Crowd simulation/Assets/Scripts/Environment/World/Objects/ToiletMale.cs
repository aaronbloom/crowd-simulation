using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class ToiletMale : Goal, ICollidable {

        public const string IdentifierStatic = "CubicleMale";
        public static readonly Vector3 SizeStatic = new Vector3(4, 4, 4);
        private static readonly Quaternion initialRotationOffSet = Quaternion.Euler(90, 180, 180);
        private static readonly Vector3 initialPositionOffSet = new Vector3(0, -2, 0);

        /// <summary>
        /// Creates new ToiletMale Object
        /// </summary>
        public ToiletMale() {
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
    }
}