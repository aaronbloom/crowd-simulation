using System;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    class WorldBuilderCursor {
        private WorldObject primaryCursor;
        private WorldObject secondCursor;
        private readonly Material cursorMaterial;
        private readonly Material invalidCursorMaterial;
        private readonly Vector3 cursorHeight = new Vector3(0, 0.2f, 0);

        private string currentItem;
        private Vector3 startPlacement;
        private bool startedPlacement = false;

        private readonly WorldBuilderPlacement worldBuilderPlacement;

        public WorldBuilderCursor() {
            worldBuilderPlacement = new WorldBuilderPlacement();
            cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
            invalidCursorMaterial = Resources.Load("Materials/InvalidCursor", typeof(Material)) as Material;
        }

        public void Update(Vector3 groundPosition) {
            if (primaryCursor != null) {
                if (secondCursor != null) { Object.Destroy(secondCursor.GameObject); }
                if (startedPlacement) {
                    UpdateSecondaryCursor(groundPosition);
                } else {
                    UpdatePrimaryCursor(groundPosition);
                }
            }
        }

        public void UpdatePrimaryCursor(Vector3 groundPosition) {
            if (primaryCursor.GameObject != null) {
                if (!primaryCursor.GridPlaceable) { // wall placement
                    Vector3 position;
                    if (worldBuilderPlacement.WallPlacement(out position, primaryCursor.Size)) {
                        primaryCursor.GameObject.transform.position =
                            Environment.Environment.PositionToLocation(position,
                            primaryCursor.Size) + cursorHeight;
                        SetCursorValid(primaryCursor);
                    } else {
                        primaryCursor.GameObject.transform.position =
                            Environment.Environment.PositionToLocation(groundPosition, primaryCursor.Size) + cursorHeight;
                        SetCursorInvalid(primaryCursor);
                    }
                } else { // floor placement
                    primaryCursor.GameObject.transform.position =
                        Environment.Environment.PositionToGridLocation(groundPosition, primaryCursor.Size) + cursorHeight;
                }
            }
        }

        public void UpdateSecondaryCursor(Vector3 groundPosition) { //secondary cursor for drag to place
            secondCursor = NewCursor(WorldObject.DetermineObject(currentItem), groundPosition);
            Vector3 mousePosition = groundPosition;

            Vector3 secondCursorPosition;
            var xDiff = startPlacement.x - mousePosition.x;
            var zDiff = startPlacement.z - mousePosition.z;
            if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff)) {
                secondCursorPosition = startPlacement + new Vector3(-xDiff, 0, 0);
            }
            else {
                secondCursorPosition = startPlacement + new Vector3(0, 0, -zDiff);
            }

            secondCursor.GameObject.transform.position =
                Environment.Environment.PositionToLocation(secondCursorPosition,
                    primaryCursor.Size) + cursorHeight;
        }

        public void SetPlacementObject(string objectName, Vector3 groundPosition) {
            currentItem = objectName;
            DestroyCursors();
            primaryCursor = NewCursor(WorldObject.DetermineObject(objectName), groundPosition);
        }

        public void StartPlaceWorldObject(Vector3 groundPosition) {
            if (currentItem != null) {
                if (UserWorldBuilder.NotOverUI()) {
                    if (!primaryCursor.GridPlaceable) {
                        Vector3 position;
                        if (worldBuilderPlacement.WallPlacement(out position, primaryCursor.Size)) {
                            startPlacement = position;
                            startedPlacement = true;
                        } else {
                            startPlacement = groundPosition;
                            startedPlacement = true;
                        }
                    } else {
                        startPlacement = groundPosition;
                        startedPlacement = true;

                    }
                }
            }
        }

        public void EndPlaceWorldObject(Vector3 groundPosition) {
            if (currentItem != null) {
                if (UserWorldBuilder.NotOverUI()) {
                    if (startedPlacement) {

                        Vector3 endPlacement;
                        if (!primaryCursor.GridPlaceable) {
                            Vector3 position;
                            if (worldBuilderPlacement.WallPlacement(out position, primaryCursor.Size)) {
                                endPlacement = position;
                                worldBuilderPlacement.PlaceLine(startPlacement, endPlacement, currentItem);
                            }
                        } else {
                            endPlacement = groundPosition;
                            worldBuilderPlacement.PlaceLine(startPlacement, endPlacement, currentItem);
                        }
                    }
                }
            }
            startedPlacement = false;
        }

        public void DestroyCursors() {
            if (primaryCursor != null) {
                Object.Destroy(primaryCursor.GameObject);
            }
        }

        private void SetCursorValid(WorldObject cursor) {
            cursor.GameObject.GetComponent<Renderer>().material = cursorMaterial;
        }

        private void SetCursorInvalid(WorldObject cursor) {
            cursor.GameObject.GetComponent<Renderer>().material = invalidCursorMaterial;
        }

        private WorldObject NewCursor(WorldObject worldObject, Vector3 groundPosition) {
            var cursor = WorldObject.Initialise(worldObject, groundPosition);
            SetCursorValid(cursor);
            Collider collider = cursor.GameObject.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
            return cursor;
        }
    }
}
