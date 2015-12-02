using UnityEngine;
using System.Collections;
using Assets.Scripts;
using Assets.Scripts.WorldObjects;

public class UserWorldBuilder : MonoBehaviour {

    private Object ghostedItemCursor;
    private const float wallSize = 4;
    private World world;

    void Start() {
        ghostedItemCursor = MonoBehaviour.Instantiate(Resources.Load("Prefabs/WallCursor"));
        world = BootStrapper.EnvironmentManager.CurrentEnvironment.World;
    }

    void Update() {
        UpdateCursorPosition();

        if (Input.GetMouseButtonDown(0)) { //left mouse clicked
            Place<Wall>(MousePositionToGroundPosition());
        }
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
        ((GameObject) ghostedItemCursor).transform.position = PositionToGridPosition(MousePositionToGroundPosition(), wallSize);

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
        var location = PositionToGridPosition(position, wallSize);
        T worldObject = new T();
        worldObject.GameObject = (GameObject) BootStrapper.Initialise("Wall", location, Quaternion.identity);
        world.Objects.Add(worldObject);
    }

    void OnDestroy() {
        GameObject.Destroy(ghostedItemCursor);
    }

}
