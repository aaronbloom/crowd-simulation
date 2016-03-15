using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {

    [Serializable]
    class WorldObjectBuildInfo {

        public string type { get; set; }
        public Vector3 position { get; set; }
        public Vector3 wallNormal { get; set; }

        public WorldObjectBuildInfo(WorldObject obj) {
            type = obj.Identifier;
            position = obj.GameObject.transform.position;
        }
    }
}
