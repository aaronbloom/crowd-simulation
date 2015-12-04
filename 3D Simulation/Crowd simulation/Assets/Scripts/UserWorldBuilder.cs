using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.WorldObjects;
using Object = UnityEngine.Object;

public class UserWorldBuilder {

    private WorldObject ghostedItemCursor;
    private readonly Material cursorMaterial;
    private const float cursorSize = 4;
    private readonly World world;
    private string currentItem;

    public UserWorldBuilder() {
        world = BootStrapper.EnvironmentManager.CurrentEnvironment.World;
        cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
    }

    public void Update() {
        UpdateCursorPosition();

        if (Input.GetMouseButtonDown(0)) { //left mouse clicked
            if (currentItem != null) {
                Place(DetermineObject(currentItem), MousePositionToGroundPosition());
            }
        }
    }

    public void SetCurrentPlacementObject(string objectName) {
        currentItem = objectName;
        ghostedItemCursor = WorldObjectInitialise(DetermineObject(currentItem), MousePositionToGroundPosition());
        ghostedItemCursor.GameObject.GetComponent<Renderer>().material = cursorMaterial;
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
            case "Goal":
                return new Goal();
        }
        return null;
    }

    private static Vector3 MousePositionToGroundPosition() {
        Plane plane = new Plane(Vector3.up, Vector3.zero); // floor
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        float distance;
        if (plane.Raycast(ray, out distance)) {
            return ray.GetPoint(distance);
        }
        return Vector3.zero;
    }

    private void UpdateCursorPosition() {
        if (ghostedItemCursor != null) {
            ghostedItemCursor.GameObject.transform.position
                = PositionToGridPosition(MousePositionToGroundPosition(), cursorSize);
        }
    }

    private static Vector3 PositionToGridPosition(Vector3 position, float objectSize) {
        var gridPosition = position;
        gridPosition -= Vector3.one * (objectSize / 2);
        gridPosition /= objectSize;
        gridPosition = new Vector3(Mathf.Round(gridPosition.x), Mathf.Round(gridPosition.y), Mathf.Round(gridPosition.z));
        gridPosition *= objectSize;
        gridPosition += Vector3.one * (objectSize / 2);
        gridPosition.y = objectSize / 2; // so it sits at ground level
        return gridPosition;
    }

    private void Place(WorldObject worldObject, Vector3 position) {
        var location = PositionToGridPosition(position, worldObject.Size);
        world.Objects.Add(WorldObjectInitialise(worldObject, location));
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
