using System;
using Assets.Scripts.Boid;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class ToiletMale : Goal, Collidable {
        public const string IdentifierStatic = "CubicleMale";
        public static Vector3 SizeStatic = new Vector3(4, 4, 4);

        public ToiletMale() : base() {
            this.Identifier = IdentifierStatic;
            this.InitialRotationOffSet = Quaternion.Euler(90, 180, 180);
            this.InitialPositionOffSet = new Vector3(0, -2, 0);
            this.Size = SizeStatic;
            this.GridPlaceable = false;
        }

        public WorldObject getObject() {
            return this;
        }
    }
}