namespace Assets.Scripts.Environment.World.Objects {
    public class Wall : WorldObject, Collidable {

        public Wall() : base() {
            this.Identifier = "Wall";
            this.Size = 4;
        }

    }
}
