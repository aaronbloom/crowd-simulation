using System;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Environment.Save {

    /// <summary>
    /// Contains all the necessary information to place an object in an environment
    /// </summary>
    [Serializable]
    public class WorldObjectBuildInfo {

        public string type { get; set; }
        public SerialisableVector3 position { get; set; }
        public SerialisableVector3 wallNormal { get; set; }

        /// <summary>
        /// creates a new build info object from an existing WorldObject
        /// </summary>
        /// <param name="obj">The WorldObject to extract info from</param>
        public WorldObjectBuildInfo(WorldObject obj) {
            type = obj.Identifier;
            position = new SerialisableVector3(obj.GameObject.transform.position);
            wallNormal = new SerialisableVector3(obj.InitialWallNormal);
        }
    }
}
