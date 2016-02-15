using System;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Toilet : Goal, Collidable {

        public Toilet() : base() {
            this.Identifier = "Toilet";
            this.InitialRotationOffSet = Quaternion.Euler(-90, 0, 180);
            this.InitialPositionOffSet = new Vector3(0, 0, 0);
            this.Size = new Vector3(4, 4, 4);
            this.GridPlaceable = false;
        }

        public WorldObject getObject() {
            return this;
        }
    }
}