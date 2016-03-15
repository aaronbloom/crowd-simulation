using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment.World.Objects;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.Environment.Save
{
    public interface IBuilder {
        void Place(WorldObject worldObject, Vector3 position, Vector3 wallNormal);
    }
}
