using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
    internal class WorldBuilderCursor {

        private static readonly Material cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
        private static readonly Material invalidCursorMaterial = Resources.Load("Materials/InvalidCursor", typeof(Material)) as Material;

        private readonly WorldBuilderPlacement worldBuilderPlacement;

        private WorldObject primaryCursor;
        private WorldObject secondCursor;
        private string currentItem;
        private Vector3 startPlacement;
        private bool startedPlacement;
        private Vector3 currentPlacementNormal;

        /// <summary>
        /// Creates new World builder cursor
        /// </summary>
        public WorldBuilderCursor() {
            worldBuilderPlacement = new WorldBuilderPlacement();
        }

        /// <summary>
        /// Updates Cursor
        /// </summary>
        /// <param name="groundPosition">new ground positon</param>
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

        /// <summary>
        /// Updates primary cursor
        /// </summary>
        /// <param name="groundPosition">new ground position</param>
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

        /// <summary>
        /// updates ghost cursor
        /// </summary>
        /// <param name="groundPosition">current ground position</param>
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

        /// <summary>
        /// Sets the current placement object
        /// </summary>
        /// <param name="objectName">new placement object</param>
        /// <param name="groundPosition">current ground position</param>
        public void SetPlacementObject(string objectName, Vector3 groundPosition) {
            currentItem = objectName;
            DestroyCursors();
            primaryCursor = NewCursor(WorldObject.DetermineObject(objectName), groundPosition);
        }

        /// <summary>
        /// Starts placement of the current world object
        /// </summary>
        /// <param name="groundPosition">the position to start placement</param>
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

        /// <summary>
        /// Ends placement of the current world object
        /// </summary>
        /// <param name="groundPosition">the position to end placement</param>
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

        /// <summary>
        /// Destroy primary cursor
        /// </summary>
        public void DestroyCursors() {
            if (primaryCursor != null) {
                Object.Destroy(primaryCursor.GameObject);
            }
        }
        
        /// <summary>
        /// Sets a cursor material to green
        /// </summary>
        /// <param name="cursor">cursor</param>
        private void SetCursorValid(WorldObject cursor) {
            cursor.GameObject.GetComponent<Renderer>().material = cursorMaterial;
        }

        /// <summary>
        /// sets a cursor material to red
        /// </summary>
        /// <param name="cursor">cursor</param>
        private void SetCursorInvalid(WorldObject cursor) {
            cursor.GameObject.GetComponent<Renderer>().material = invalidCursorMaterial;
        }

        /// <summary>
        /// Sets height of cursor
        /// </summary>
        /// <param name="cursor">cursor</param>
        private void SetCursorHeight(WorldObject cursor) {
            var position = cursor.GameObject.transform.position;
            cursor.GameObject.transform.position = new Vector3(position.x, cursor.CursorHeight.y, position.z);
        }

        /// <summary>
        /// Creates a new cursor with the given worldobject
        /// </summary>
        /// <param name="worldObject">the world object the cursor should look like</param>
        /// <param name="groundPosition">the position of the cursor</param>
        /// <returns>the new cursor object</returns>
        private WorldObject NewCursor(WorldObject worldObject, Vector3 groundPosition) {
            var cursor = WorldObject.Initialise(worldObject, groundPosition, Vector3.zero);
            SetCursorValid(cursor);
            Collider collider = cursor.GameObject.GetComponent<Collider>();
            if (collider != null) collider.enabled = false;
            return cursor;
        }
    }
}
