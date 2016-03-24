using UnityEngine;

namespace Assets.Scripts.Environment.World.Objects {
    public class Wall : WorldObject, ICollidable {

        public const string IdentifierStatic = "Wall";
        public static readonly Vector3 SizeStatic = new Vector3(1, 4, 1);
        private static readonly Vector3 cursorHeight = new Vector3(0, 2.1f, 0);

        public Wall() {
            this.Identifier = IdentifierStatic;
            this.Size = SizeStatic;
            this.GridPlaceable = true;
            this.CursorHeight = cursorHeight;
        }

        WorldObject ICollidable.GetObject() {
            return this;
        }
    }
}
