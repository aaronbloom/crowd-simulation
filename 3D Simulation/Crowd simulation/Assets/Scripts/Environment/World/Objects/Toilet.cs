using System;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Toilet : WorldObject, Collidable {

        public Toilet() : base() {
            this.Identifier = "obj_potty2";
            this.InitialRotationOffSet = Quaternion.Euler(0, 0, 0);
            this.Size = 4;
        }

        public WorldObject getObject() {
            return this;
        }
    }
}