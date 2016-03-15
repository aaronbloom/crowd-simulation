using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Environment.Save {

    public class SaveableEnvironment {

        private List<WorldObjectBuildInfo> saveableItems = new List<WorldObjectBuildInfo>();

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
                builder.Place(WorldObject.DetermineObject(savedItem.type), savedItem.position, savedItem.wallNormal);
            }
        }

        

    }
}
