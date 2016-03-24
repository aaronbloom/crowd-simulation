using System;
using Assets.Scripts.Environment.Save;
using Assets.Scripts.UserInterface;
using UnityEngine;

namespace Assets.Scripts.Environment
{
    public class EnvironmentManager {

        private static EnvironmentManager shared;
        public Environment CurrentEnvironment { get; private set; }

        // Setup the static instance of this EnvironmentManager
        public EnvironmentManager() {
            initSingleton();
        }

        public void InitialiseEnvironment(Vector3 bounds) {
            CurrentEnvironment = new Environment(bounds);
            CurrentEnvironment.Setup();
        }

        public void InitialiseEnvironment(int size) {
            int environmentHeight = 50;
            Vector3 bounds = new Vector3(size, environmentHeight, size);
            Shared().InitialiseEnvironment(bounds);
        }

        public void LoadEnvironmentFromFile(string fileName) {
            var savedEnv = SystemSaveFolder.LoadFileFromFolder(fileName) as SaveableEnvironment;
            if (savedEnv != null) {
                LoadEnvironment(savedEnv);
            }
        }

        public void LoadEnvironment(SaveableEnvironment savedEnvironment) {
            BootStrapper.EnvironmentManager.InitialiseEnvironment(savedEnvironment.environmentBounds.Vector3());
            savedEnvironment.BuildWorldWith(new WorldBuilderPlacement());
        }

        private void initSingleton() {
            // Set the 'shared' variable as this environment manager
            if (shared == null) {
                shared = this;
            } else {
                throw new InvalidOperationException("Singleton already setup");
            }
        }

        // Return the static instance of this EnvironmentManager
        public static EnvironmentManager Shared() {
            return shared;
        }

        public void OnDrawGizmos() {
            if (CurrentEnvironment != null) CurrentEnvironment.OnDrawGizmos();
        }

    }
}
