using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using UnityEngineInternal;

namespace Assets.Scripts.Environment.Save {

    [Serializable]
    public class SaveableEnvironment
    {

        public SerialisableVector3 environmentBounds { get; private set; }
        public List<WorldObjectBuildInfo> saveableItems { get; private set; }

        public SaveableEnvironment(Vector3 environmentBounds) {
            this.environmentBounds = new SerialisableVector3(environmentBounds);
            saveableItems = new List<WorldObjectBuildInfo>();
        }

        public void SaveWorldObjects(List<WorldObject> worldObjects) {
            foreach (var worldObject in worldObjects) {
                SaveWorldObject(worldObject);
            }
        }

        public void SaveWorldObject(WorldObject worldObject) {
            saveableItems.Add(new WorldObjectBuildInfo(worldObject));
        }

        public void BuildWorldWith(IBuilder builder) {
            foreach (var savedItem in saveableItems) {
                builder.Place(WorldObject.DetermineObject(savedItem.type), savedItem.position.Vector3(), savedItem.wallNormal.Vector3());
            }
        }

        

    }
}
