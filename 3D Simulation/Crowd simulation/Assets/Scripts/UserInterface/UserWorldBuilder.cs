using System;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    public class UserWorldBuilder {

        private WorldObject primaryCursor;
        private WorldObject secondCursor;
        private readonly Material cursorMaterial;
        private string currentItem;
        private Vector3 startPlacement;
        private bool startedPlacement = false;
        private readonly Vector3 cursorHeight = new Vector3(0, 0.2f, 0);

        public UserWorldBuilder() {
            cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
        }

        public void StartPlaceWorldObject() {
            if (currentItem != null) {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) { //is mouse pointer not over a menu ui
                    startPlacement = MousePositionToGroundPosition();
                    startedPlacement = true;
                }
            }
        }

        public void EndPlaceWorldObject() {
            if (currentItem != null) {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) { //is mouse pointer not over a menu ui
                    if (startedPlacement) {
                        PlaceLine(startPlacement, MousePositionToGroundPosition(), currentItem);
                    }
                }
            }
            startedPlacement = false;
        }

        public void SetCurrentPlacementObject(string objectName) {
            currentItem = objectName;
            DestroyCursors();
            primaryCursor = NewCursor(DetermineObject(currentItem));
        }

        public void UpdateCursorPosition() {
            if (primaryCursor != null) {
                if (secondCursor != null) { Object.Destroy(secondCursor.GameObject); }
                if (startedPlacement) {
                    secondCursor = NewCursor(DetermineObject(currentItem));
                    Vector3 mousePosition = MousePositionToGroundPosition();

                    Vector3 secondCursorPosition;
                    var xDiff = startPlacement.x - mousePosition.x;
                    var zDiff = startPlacement.z - mousePosition.z;
                    if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff)) {
                        secondCursorPosition = startPlacement + new Vector3(-xDiff, 0, 0);
                    } else {
                        secondCursorPosition = startPlacement + new Vector3(0, 0, -zDiff);
                    }

                    secondCursor.GameObject.transform.position 
                        = Environment.Environment.PositionToGridPosition(secondCursorPosition, primaryCursor.Size) + cursorHeight;
                } else {
                    if (primaryCursor.GameObject != null) {
                        primaryCursor.GameObject.transform.position
                            = Environment.Environment.PositionToGridPosition(
                                MousePositionToGroundPosition(),
                                primaryCursor.Size) + cursorHeight;
                    }
                }
            }
        }

        public void DestroyCursors() {
            if (primaryCursor != null) {
                    Object.Destroy(primaryCursor.GameObject);
            }
        }

        private WorldObject NewCursor(WorldObject worldObject) {
            var cursor = WorldObject.Initialise(worldObject, MousePositionToGroundPosition());
            cursor.GameObject.GetComponent<Renderer>().material = cursorMaterial;
            return cursor;
        }

        private WorldObject[] PlaceLine(Vector3 start, Vector3 end, String objectName) {
            Vector3 step;
            var xDiff = start.x - end.x;
            var zDiff = start.z - end.z;
            int largerDiff;
            if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff)) {
                step = new Vector3(-Mathf.Sign(xDiff), 0, 0);
                largerDiff = (int) Mathf.Abs(xDiff);
            } else {
                step = new Vector3(0, 0, -Mathf.Sign(zDiff));
                largerDiff = (int) Mathf.Abs(zDiff);
            }
            Vector3 position = start;
            WorldObject[] createdWorldObjects = new WorldObject[largerDiff + 1];
            for (int i = 0; i <= largerDiff; i++) {
                var currentWorldObject = DetermineObject(objectName);
                createdWorldObjects[i] = currentWorldObject;
                BootStrapper.EnvironmentManager.CurrentEnvironment.Place(currentWorldObject, position);
                position += step;
            }
            return createdWorldObjects;
        }

        private static WorldObject DetermineObject(string objectName) {
            switch (objectName) {
                case Wall.IdentifierStatic:
                    return new Wall();
                case Entrance.IdentifierStatic:
                    return new Entrance();
                case Toilet.IdentifierStatic:
                    return new Toilet();
                case Stage.IdentifierStatic:
                    return new Stage();
                case Bar.IdentifierStatic:
                    return new Bar();
            }
            return null;
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
    }
}
