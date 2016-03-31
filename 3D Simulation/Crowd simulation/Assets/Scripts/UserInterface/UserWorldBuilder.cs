using System.Collections.Generic;
using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UserInterface {
    public class UserWorldBuilder {

        private readonly WorldBuilderCursor cursor;
        private bool deletionToolActive;

        public UserWorldBuilder() {
            cursor = new WorldBuilderCursor();
            deletionToolActive = false;
        }

        /// <summary>
        /// Begins placement of the currently selected world object
        /// </summary>
        public void StartPlaceWorldObject() {
            EventSystem.current.SetSelectedGameObject(null, null);
            if (deletionToolActive) {
                this.DeleteWorldObject();
            } else {
                cursor.StartPlaceWorldObject(MousePositionToGroundPosition());
            }
        }

        /// <summary>
        /// Ends the placement of the currently selected world object
        /// </summary>
        public void EndPlaceWorldObject() {
            if (!deletionToolActive) {
                cursor.EndPlaceWorldObject(MousePositionToGroundPosition());
            }
        }

        /// <summary>
        /// Sets the currently selected World object
        /// </summary>
        /// <param name="objectName">the world object name</param>
        public void SetCurrentPlacementObject(string objectName) {
            EventSystem.current.SetSelectedGameObject(null, null);
            deletionToolActive = false;
            cursor.SetPlacementObject(objectName, MousePositionToGroundPosition());
        }

        /// <summary>
        /// Enables the deletion tool
        /// </summary>
        public void EnableDeletionTool() {
            EventSystem.current.SetSelectedGameObject(null, null);
            deletionToolActive = true;
            cursor.DestroyCursors();
        }

        /// <summary>
        /// Deletes a particular world object
        /// </summary>
        public void DeleteWorldObject() {
            if (NotOverUI()) {
                RaycastHit hit;
                GameObject gameObject;
                if(Raycast(out hit,out gameObject)) {
                    List<WorldObject> worldObjects = EnvironmentManager.Shared().CurrentEnvironment.World.Objects;
                    foreach (WorldObject worldObject in worldObjects) {
                        if (worldObject.GameObject.Equals(gameObject)) {
                            EnvironmentManager.Shared().CurrentEnvironment.World.RemoveObject(worldObject);
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates the current cursor position
        /// </summary>
        public void UpdateCursorPosition() {
            cursor.Update(MousePositionToGroundPosition());
        }

        /// <summary>
        /// Is the mouse pointer not over a menu ui
        /// </summary>
        /// <returns>true if mouse not over UI</returns>
        public static bool NotOverUI() {
            EventSystem.current.SetSelectedGameObject(null, null);
            return !EventSystem.current.IsPointerOverGameObject(-1);
        }

        /// <summary>
        /// Casts a ray to see if it hit a gameobject
        /// </summary>
        /// <param name="hit">The raycastHit</param>
        /// <param name="gameObject">the gameobject that was hit</param>
        /// <returns>True if gameobject hit</returns>
        public static bool Raycast(out RaycastHit hit, out GameObject gameObject) {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                gameObject = hit.transform.gameObject;
                return true;
            }
            gameObject = null;
            return false;
        }

        /// <summary>
        /// Sees where the mouse is on the ground plane
        /// </summary>
        /// <returns>the position of the mouse on the ground</returns>
        private static Vector3 MousePositionToGroundPosition() {
            Plane plane = new Plane(Vector3.up, Vector3.zero); // floor
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;
            return plane.Raycast(ray, out distance) ? ray.GetPoint(distance) : Vector3.zero;
        }

        public void Destroy() {
            cursor.DestroyCursors();
        }
    }
}
