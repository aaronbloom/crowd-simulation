using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.WorldObjects;
using Object = UnityEngine.Object;

public class UserWorldBuilder {

    private Object ghostedItemCursor;
    private const float cursorSize = 4;
    private World world;
    private string currentItem;

    public UserWorldBuilder() {
        ghostedItemCursor = MonoBehaviour.Instantiate(Resources.Load("Prefabs/WallCursor"));
        world = BootStrapper.EnvironmentManager.CurrentEnvironment.World;
    }

    public void Update() {
        UpdateCursorPosition();

        if (Input.GetMouseButtonDown(0)) { //left mouse clicked
            switch (currentItem) {
                case "Wall":
                    Place<Wall>(MousePositionToGroundPosition());
                    break;
                case "Entrance":
                    Place<Entrance>(MousePositionToGroundPosition());
                    break;
                case "Goal":
                    Place<Goal>(MousePositionToGroundPosition());
                    break;
            }
        }
    }

    public void SetCurrentPlacementObject(string objectName) {
        currentItem = objectName;
    }

    public void Destroy() {
        Object.Destroy(ghostedItemCursor);
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
        ((GameObject) ghostedItemCursor).transform.position = PositionToGridPosition(MousePositionToGroundPosition(), cursorSize);
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

    private void Place<T>(Vector3 position) where T : WorldObject, new() {
        T worldObject = new T();
        var location = PositionToGridPosition(position, worldObject.Size);
        worldObject.GameObject = (GameObject) BootStrapper.Initialise(
            worldObject.Identifier,
            location + worldObject.InitialPositionOffSet,
            worldObject.InitialRotationOffSet
            );
        world.Objects.Add(worldObject);
    }
}
