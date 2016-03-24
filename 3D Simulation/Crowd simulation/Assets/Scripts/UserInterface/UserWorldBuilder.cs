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

        public void StartPlaceWorldObject() {
            EventSystem.current.SetSelectedGameObject(null, null);
            if (deletionToolActive) {
                this.DeleteWorldObject();
            } else {
                cursor.StartPlaceWorldObject(MousePositionToGroundPosition());
            }
        }

        public void EndPlaceWorldObject() {
            if (!deletionToolActive) {
                cursor.EndPlaceWorldObject(MousePositionToGroundPosition());
            }
        }

        public void SetCurrentPlacementObject(string objectName) {
            EventSystem.current.SetSelectedGameObject(null, null);
            deletionToolActive = false;
            cursor.SetPlacementObject(objectName, MousePositionToGroundPosition());
        }

        public void EnableDeletionTool() {
            EventSystem.current.SetSelectedGameObject(null, null);
            deletionToolActive = true;
            cursor.DestroyCursors();
        }

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

        public void UpdateCursorPosition() {
            cursor.Update(MousePositionToGroundPosition());
        }

        public static bool NotOverUI() { // is mouse pointer not over a menu ui
            EventSystem.current.SetSelectedGameObject(null, null);
            return !EventSystem.current.IsPointerOverGameObject(-1);
        }

        public static bool Raycast(out RaycastHit hit, out GameObject gameObject) {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                gameObject = hit.transform.gameObject;
                return true;
            }
            gameObject = null;
            return false;
        }

        private static Vector3 MousePositionToGroundPosition() {
            Plane plane = new Plane(Vector3.up, Vector3.zero); // floor
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;
            if (plane.Raycast(ray, out distance)) {
                return ray.GetPoint(distance);
            }
            return Vector3.zero;
        }

        public void Destroy() {
            cursor.DestroyCursors();
        }
    }
}
