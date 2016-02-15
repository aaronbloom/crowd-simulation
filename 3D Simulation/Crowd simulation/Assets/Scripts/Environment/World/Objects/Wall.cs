using System;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Wall : WorldObject, Collidable {

        public Wall() : base() {
            this.Identifier = "Wall";
            this.Size = new Vector3(1, 4, 1);
            this.GridPlaceable = true;
        }

        WorldObject Collidable.getObject() {
            return this;
        }

    }
}
