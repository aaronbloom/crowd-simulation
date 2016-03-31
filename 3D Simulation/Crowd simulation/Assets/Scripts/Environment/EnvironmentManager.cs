using System;
using Assets.Scripts.Environment.Save;
using Assets.Scripts.UserInterface;
using UnityEngine;

namespace Assets.Scripts.Environment {
    public class EnvironmentManager {

        public Environment CurrentEnvironment { get; private set; }
        private static EnvironmentManager shared;

        // Setup the static instance of this EnvironmentManager
        public EnvironmentManager() {
            initSingleton();
        }

        // Return the static instance of this EnvironmentManager
        public static EnvironmentManager Shared() {
            return shared;
        }

        /// <summary>
        /// Initialises the environment
        /// </summary>
        /// <param name="bounds">the size of the environment</param>
        public void InitialiseEnvironment(Vector3 bounds) {
            CurrentEnvironment = new Environment(bounds);
            CurrentEnvironment.Setup();
        }

        /// <summary>
        /// Intialises the environment
        /// </summary>
        /// <param name="size">the size of the environment</param>
        public void InitialiseEnvironment(int size) {
            int environmentHeight = 50;
            Vector3 bounds = new Vector3(size, environmentHeight, size);
            Shared().InitialiseEnvironment(bounds);
        }

        /// <summary>
        /// loads the environment from a file
        /// </summary>
        /// <param name="fileName">the file to load</param>
        public void LoadEnvironmentFromFile(string fileName) {
            var savedEnv = SystemSaveFolder.LoadFileFromFolder(fileName) as SaveableEnvironment;
            if (savedEnv != null) {
                LoadEnvironment(savedEnv);
            }
        }

        /// <summary>
        /// Loads an environment from a deserialised file
        /// </summary>
        /// <param name="savedEnvironment">the deserialised file</param>
        public void LoadEnvironment(SaveableEnvironment savedEnvironment) {
            BootStrapper.EnvironmentManager.InitialiseEnvironment(savedEnvironment.environmentBounds.Vector3());
            savedEnvironment.BuildWorldWith(new WorldBuilderPlacement());
        }

        /// <summary>
        /// Draws graph debugging lines
        /// </summary>
        public void OnDrawGizmos() {
            if (CurrentEnvironment != null) CurrentEnvironment.OnDrawGizmos();
        }

        /// <summary>
        /// Enforces the singleton nature of Environment manager
        /// </summary>
        private void initSingleton() {
            // Set the 'shared' variable as this environment manager
            if (shared == null) {
                shared = this;
            } else {
                throw new InvalidOperationException("Singleton already setup");
            }
        }
    }
}
