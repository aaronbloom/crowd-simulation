using System;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Toilet : Goal, Collidable {
        public const string IdentifierStatic = "Toilet";
        public static Vector3 SizeStatic = new Vector3(4, 4, 4);

        public Toilet() : base() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = Quaternion.Euler(-90, 0, 180);
            this.InitialPositionOffSet = new Vector3(0, 0, 0);
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

        public WorldObject getObject() {
            return this;
        }
    }
}