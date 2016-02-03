﻿using Assets.Scripts.Environment;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    public class UserWorldBuilder {

        private WorldObject ghostedItemCursor;
        private readonly Material cursorMaterial;
        private const float cursorSize = 4;
        private readonly Environment.World.World world;
        private string currentItem;

        public UserWorldBuilder() {
            world = BootStrapper.EnvironmentManager.CurrentEnvironment.World;
            cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
        }

        public void PlaceWorldObject() {
            if (currentItem != null) {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) { //is mouse pointer not over a menu ui
                    Place(DetermineObject(currentItem), MousePositionToGroundPosition());
                }
            }
        }

        public void SetCurrentPlacementObject(string objectName) {
            currentItem = objectName;
            Destroy();
            ghostedItemCursor = WorldObjectInitialise(DetermineObject(currentItem), MousePositionToGroundPosition());
            ghostedItemCursor.GameObject.GetComponent<Renderer>().material = cursorMaterial;
        }

        public void UpdateCursorPosition() {
            if (ghostedItemCursor != null) {
                ghostedItemCursor.GameObject.transform.position
                    = PositionToGridPosition(MousePositionToGroundPosition(), cursorSize);
            }
        }

        public void Destroy() {
            if (ghostedItemCursor != null) {
                Object.Destroy(ghostedItemCursor.GameObject);
            }
        }

        private WorldObject DetermineObject(string objectName) {
            switch (currentItem) {
                case "Wall":
                    return new Wall();
                case "Entrance":
                    return new Entrance();
                case "Toilet":
                    return new Toilet();
                case "Stage":
                    return new Stage();
                case "Bar":
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

        private static Vector3 PositionToGridPosition(Vector3 position, float objectSize) {
            var gridPosition = position;
            gridPosition -= Vector3.one * (objectSize / 2);
            gridPosition /= objectSize;
            gridPosition = new Vector3(Mathf.Round(gridPosition.x), Mathf.Round(gridPosition.y), Mathf.Round(gridPosition.z));
            gridPosition *= objectSize;
            gridPosition += Vector3.one * (objectSize / 2);
            gridPosition.y = objectSize / 2; // so it sits at ground level
            Vector3 bounds = EnvironmentManager.Shared().CurrentEnvironment.Bounds;
            Vector3 origin = EnvironmentManager.Shared().CurrentEnvironment.Origin;
            return ConstrainVector(gridPosition, origin, bounds, objectSize / 2);
        }

        private static Vector3 ConstrainVector(Vector3 position, Vector3 origin, Vector3 bounds, float halfObjectSize) {
            position.x = Mathf.Clamp(position.x, origin.x + halfObjectSize, origin.x + bounds.x - halfObjectSize);
            position.y = Mathf.Clamp(position.y, origin.y + halfObjectSize, origin.y + bounds.y - halfObjectSize);
            position.z = Mathf.Clamp(position.z, origin.z + halfObjectSize, origin.z + bounds.z - halfObjectSize);
            return position;
        }

        private void Place(WorldObject worldObject, Vector3 position) {
            var location = PositionToGridPosition(position, worldObject.Size);
            if (!world.AddObject(WorldObjectInitialise(worldObject, location))) {
                Debug.Log("Could not add new world object - Already occupied");
                worldObject.Destroy();
            }
        }

        private WorldObject WorldObjectInitialise(WorldObject worldObject, Vector3 position) {
            worldObject.GameObject = (GameObject)BootStrapper.Initialise(
                worldObject.Identifier,
                position + worldObject.InitialPositionOffSet,
                worldObject.InitialRotationOffSet
                );
            return worldObject;
        }
    }
}
