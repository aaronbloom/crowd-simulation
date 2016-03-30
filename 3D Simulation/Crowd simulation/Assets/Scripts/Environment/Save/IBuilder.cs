using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {

    /// <summary>
    /// An interface which can be implemented to create a world loader
    /// </summary>
    public interface IBuilder {

        /// <summary>
        /// Places a certain world object with a certain position and rotation
        /// </summary>
        /// <param name="worldObject">object type</param>
        /// <param name="position">position</param>
        /// <param name="wallNormal">normal used to calculate rotation</param>
        void Place(WorldObject worldObject, Vector3 position, Vector3 wallNormal);

    }
}
