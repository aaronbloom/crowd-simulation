using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Wall : WorldObject, Collidable {
        public const string IdentifierStatic = "Wall";
        public static Vector3 SizeStatic = new Vector3(1, 4, 1);

        public Wall() : base() {
            this.Identifier = IdentifierStatic;
            this.Size = SizeStatic;
            this.GridPlaceable = true;
            this.CursorHeight = new Vector3(0, 2.1f, 0);
        }

        WorldObject Collidable.getObject() {
            return this;
        }

    }
}
