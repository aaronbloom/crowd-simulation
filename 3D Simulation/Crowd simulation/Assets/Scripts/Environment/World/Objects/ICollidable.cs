namespace Assets.Scripts.Environment.World.Objects {

    /// <summary>
    /// Designates that a class should be culled from the nav graph
    /// </summary>
    public interface ICollidable  {

        WorldObject GetObject();

    }
}
