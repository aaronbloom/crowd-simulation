using System;
using System.Collections.Generic;
using Assets.Scripts.Environment;
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
        private Vector3 currentPlacementNormal;

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
                    Vector3 normal;
                    if (worldBuilderPlacement.WallPlacement(out normal, out position, primaryCursor)) {
                        primaryCursor.GameObject.transform.position =
                            Environment.Environment.PositionToLocation(position, primaryCursor.Size);
                        SetCursorHeight(primaryCursor);
                        primaryCursor.LookTowardsNormal(normal);
                        SetCursorValid(primaryCursor);
                    } else {
                        primaryCursor.GameObject.transform.position = Environment.Environment.PositionToLocation(groundPosition, primaryCursor.Size);
                        SetCursorHeight(primaryCursor);
                        SetCursorInvalid(primaryCursor);
                    }
                } else { // floor placement
                    primaryCursor.GameObject.transform.position =
                        Environment.Environment.PositionToGridLocation(groundPosition, primaryCursor.Size);
                    SetCursorHeight(primaryCursor);
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
                    primaryCursor.Size);
            SetCursorHeight(secondCursor);
            secondCursor.GameObject.transform.rotation = primaryCursor.GameObject.transform.rotation;

            if (!primaryCursor.GridPlaceable) { // wall placement
                Vector3 position;
                Vector3 normal;
                if (worldBuilderPlacement.WallPlacement(out normal, out position, primaryCursor)) {
                    secondCursor.LookTowardsNormal(normal);
                    SetCursorValid(secondCursor);
                } else {
                    SetCursorInvalid(secondCursor);
                }
            }
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
                        if (worldBuilderPlacement.WallPlacement(out currentPlacementNormal, out position, primaryCursor)) {
                            startPlacement = position;
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
                        if (!primaryCursor.GridPlaceable) {
                            Vector3 position;
                            if (worldBuilderPlacement.WallPlacement(out currentPlacementNormal, out position, primaryCursor)) {
                                Vector3 endPlacement = position;
                                worldBuilderPlacement.PlaceLine(startPlacement, endPlacement, currentPlacementNormal, currentItem);
                            } 
                        } else {
                            worldBuilderPlacement.PlaceLine(startPlacement, groundPosition, Vector3.forward, currentItem);
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

        private void SetCursorHeight(WorldObject cursor) {
            var position = cursor.GameObject.transform.position;
            cursor.GameObject.transform.position = new Vector3(position.x, cursorHeight.y, position.z);
        }

        private WorldObject NewCursor(WorldObject worldObject, Vector3 groundPosition) {
            var cursor = WorldObject.Initialise(worldObject, groundPosition, Vector3.zero);
            SetCursorValid(cursor);
            Collider collider = cursor.GameObject.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
            return cursor;
        }
    }
}
