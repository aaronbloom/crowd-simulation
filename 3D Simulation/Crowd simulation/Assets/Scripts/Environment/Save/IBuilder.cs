using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {
    public interface IBuilder {

        void Place(WorldObject worldObject, Vector3 position, Vector3 wallNormal);

    }
}
