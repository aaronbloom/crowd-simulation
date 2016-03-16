using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {

    [Serializable]
    public class WorldObjectBuildInfo {

        public string type { get; set; }
        public SerialisableVector3 position { get; set; }
        public SerialisableVector3 wallNormal { get; set; }

        public WorldObjectBuildInfo(WorldObject obj) {
            type = obj.Identifier;
            position = new SerialisableVector3(obj.GameObject.transform.position);
            wallNormal = new SerialisableVector3(obj.InitialWallNormal);
        }
    }
}
