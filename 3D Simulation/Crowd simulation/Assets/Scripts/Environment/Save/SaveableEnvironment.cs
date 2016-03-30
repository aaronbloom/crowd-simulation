using System;
using System.Collections.Generic;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.Save {

    /// <summary>
    /// A class to hold the information to build a world. Can be serialised and written to a file.
    /// </summary>
    [Serializable]
    public class SaveableEnvironment {

        public SerialisableVector3 environmentBounds { get; private set; }
        public List<WorldObjectBuildInfo> saveableItems { get; private set; }

        /// <summary>
        /// Creates a new saveableEnvironment with the bounds given by <paramref name="environmentBounds"/>
        /// </summary>
        /// <param name="environmentBounds">bounds for the environment</param>
        public SaveableEnvironment(Vector3 environmentBounds) {
            this.environmentBounds = new SerialisableVector3(environmentBounds);
            saveableItems = new List<WorldObjectBuildInfo>();
        }

        /// <summary>
        /// Stores relevant information from a list of worldObjects to save them to a file
        /// </summary>
        /// <param name="worldObjects">list of objects to save</param>
        public void SaveWorldObjects(List<WorldObject> worldObjects) {
            foreach (var worldObject in worldObjects) {
                SaveWorldObject(worldObject);
            }
        }

        /// <summary>
        /// Stores relevant information about a particular worldObject to save it to a file
        /// </summary>
        /// <param name="worldObject">the object to save</param>
        public void SaveWorldObject(WorldObject worldObject) {
            saveableItems.Add(new WorldObjectBuildInfo(worldObject));
        }

        /// <summary>
        /// Builds a world using an IBuilder from the stored information
        /// </summary>
        /// <param name="builder">The environment builder to use</param>
        public void BuildWorldWith(IBuilder builder) {
            foreach (var savedItem in saveableItems) {
                builder.Place(WorldObject.DetermineObject(savedItem.type), savedItem.position.Vector3(), savedItem.wallNormal.Vector3());
            }
        }
    }
}
