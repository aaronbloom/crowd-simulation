using System;
using Assets.Scripts.Environment.World.Objects;
using UnityEditor;
using UnityEngine;
using UnityEngineInternal;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    public class UserWorldBuilder {

        private WorldObject primaryCursor;
        private WorldObject secondCursor;
        private readonly Material cursorMaterial;
        private readonly Material invalidCursorMaterial;
        private string currentItem;
        private Vector3 startPlacement;
        private bool startedPlacement = false;
        private readonly Vector3 cursorHeight = new Vector3(0, 0.2f, 0);

        public UserWorldBuilder() {
            cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
            invalidCursorMaterial = Resources.Load("Materials/InvalidCursor", typeof (Material)) as Material;
        }

        public void StartPlaceWorldObject() {
            if (currentItem != null) {
                if (NotOverUI()) {

                    if (!primaryCursor.GridPlaceable) {
                        Vector3 position;
                        if (WallPlacement(out position)) {
                            startPlacement = position;
                            startedPlacement = true;
                        } else {
                            startPlacement = MousePositionToGroundPosition();
                            startedPlacement = true;
                        }
                    } else {
                        startPlacement = MousePositionToGroundPosition();
                        startedPlacement = true;
                    }

                }
            }
        }

        public void EndPlaceWorldObject() {
            if (currentItem != null) {
                if (NotOverUI()) {
                    if (startedPlacement) {

                        Vector3 endPlacement;
                        if (!primaryCursor.GridPlaceable) {
                            Vector3 position;
                            if (WallPlacement(out position)) {
                                endPlacement = position;
                                PlaceLine(startPlacement, endPlacement, currentItem);
                            }
                        } else {
                            endPlacement = MousePositionToGroundPosition();
                            PlaceLine(startPlacement, endPlacement, currentItem);
                        }
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
                    UpdateSecondaryCursorPosition();
                } else {
                    if (primaryCursor.GameObject != null) {
                        if (!primaryCursor.GridPlaceable) { // wall placement
                            Vector3 position;
                            if (WallPlacement(out position)) {
                                primaryCursor.GameObject.transform.position = 
                                    Environment.Environment.PositionToLocation(position,
                                    primaryCursor.Size) + cursorHeight;
                                SetCursorValid(primaryCursor);
                            } else {
                                primaryCursor.GameObject.transform.position =
                                    Environment.Environment.PositionToLocation(MousePositionToGroundPosition(),
                                        primaryCursor.Size) + cursorHeight;
                                SetCursorInvalid(primaryCursor);
                            }
                        } else { // floor placement
                            primaryCursor.GameObject.transform.position = 
                                Environment.Environment.PositionToGridLocation(MousePositionToGroundPosition(),
                                    primaryCursor.Size) + cursorHeight;
                        }
                    }
                }
            }
        }

        public void DestroyCursors() {
            if (primaryCursor != null) {
                    Object.Destroy(primaryCursor.GameObject);
            }
        }

        private bool NotOverUI() { // is mouse pointer not over a menu ui
            return !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1);
        }

        private void UpdateSecondaryCursorPosition() { //secondary cursor for drag to place
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

            secondCursor.GameObject.transform.position =
                Environment.Environment.PositionToLocation(secondCursorPosition,
                primaryCursor.Size) + cursorHeight;
        }

        private bool WallPlacement(out Vector3 position) { //is the cursor over a wall
            RaycastHit hit;
            GameObject gameObject;
            if(Raycast(out hit, out gameObject)) {

                if (gameObject.name.Contains(Wall.IdentifierStatic)) {

                    Vector3 firstWallPosition = hit.transform.position;

                    Vector3 normal = hit.normal;
                    if (normal.y == 0) { //only wall sides, not the tops
                        //perpendicular vector to normal (right vector from point of view of normal)
                        Vector3 crossRight = Vector3.Cross(Vector3.up, normal).normalized;

                        float requiredWidth = primaryCursor.Size.x;

                        Vector3 leftMostWall = firstWallPosition;
                        Vector3 rightMostWall = firstWallPosition;

                        //Scans and checks existance of walls left and right of the selected wall
                        int count = 1;
                        Vector3 scanPosition = firstWallPosition;
                        for (int i = 0; i < requiredWidth - 1; i++) {
                            if (count >= requiredWidth) break;
                            scanPosition += crossRight;
                            if (
                                BootStrapper.EnvironmentManager.CurrentEnvironment.World.AlreadyOccupied(
                                    scanPosition)) {
                                count++;
                                rightMostWall = scanPosition;
                            }
                            else {
                                scanPosition = firstWallPosition;
                                for (int j = 0; j < requiredWidth - 1; j++) {
                                    if (count >= requiredWidth) break;
                                    scanPosition -= crossRight;
                                    if (
                                        BootStrapper.EnvironmentManager.CurrentEnvironment.World
                                            .AlreadyOccupied(scanPosition)) {
                                        count++;
                                        leftMostWall = scanPosition;
                                    }
                                    else {
                                        break;
                                    }
                                }
                                break;
                            }
                        }

                        //if walls are sufficiently wide, then calculate position of wall placed world object
                        if (count >= requiredWidth) {

                            Vector3 centerWall = (leftMostWall + rightMostWall)/2;
                            Vector3 requiredDepth = normal*(primaryCursor.Size.z/2);
                            Vector3 wallOffset = normal*(Wall.SizeStatic.z/2);

                            position = centerWall + requiredDepth + wallOffset;
                            return true;
                        }
                    }
                }
            }
            position = Vector3.zero;
            return false;
        }  

        private static bool Raycast(out RaycastHit hit, out GameObject gameObject) {
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                gameObject = hit.transform.gameObject;
                return true;
            }
            gameObject = null;
            return false;
        }

        private WorldObject NewCursor(WorldObject worldObject) {
            var cursor = WorldObject.Initialise(worldObject, MousePositionToGroundPosition());
            SetCursorValid(cursor);
            Collider collider = cursor.GameObject.GetComponent<Collider>();
            if(collider != null) collider.enabled = false;
            return cursor;
        }

        private void SetCursorValid(WorldObject cursor) {
            cursor.GameObject.GetComponent<Renderer>().material = cursorMaterial;
        }

        private void SetCursorInvalid(WorldObject cursor) {
            cursor.GameObject.GetComponent<Renderer>().material = invalidCursorMaterial;
        }

        private void Place(WorldObject worldObject, Vector3 position) {
            BootStrapper.EnvironmentManager.CurrentEnvironment.Place(worldObject, position);
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
                Place(currentWorldObject, position);
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
