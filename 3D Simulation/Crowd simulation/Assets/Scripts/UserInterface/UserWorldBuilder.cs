using System;
using Assets.Scripts.Environment.World.Objects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking.Match;
using UnityEngineInternal;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    public class UserWorldBuilder {

        private WorldBuilderCursor cursor;

        public UserWorldBuilder() {
            cursor = new WorldBuilderCursor();
        }

        public void StartPlaceWorldObject() {
            cursor.StartPlaceWorldObject();
        }

        public void EndPlaceWorldObject() {
            cursor.EndPlaceWorldObject();
        }

        public void SetCurrentPlacementObject(string objectName) {
            cursor.SetPlacementObject(objectName);
        }

        public void UpdateCursorPosition() {
            cursor.Update();
        }

        public static bool NotOverUI() { // is mouse pointer not over a menu ui
            return !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);
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

        public static Vector3 MousePositionToGroundPosition() {
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
