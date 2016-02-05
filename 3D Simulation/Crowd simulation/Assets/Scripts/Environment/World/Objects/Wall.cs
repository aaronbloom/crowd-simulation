using System;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Wall : WorldObject, Collidable {

        public Wall() : base() {
            this.Identifier = "Wall";
            this.Size = new Vector3(4, 4, 4);
        }

        WorldObject Collidable.getObject() {
            return this;
        }

    }
}
