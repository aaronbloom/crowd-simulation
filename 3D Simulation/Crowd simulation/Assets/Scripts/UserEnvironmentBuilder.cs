using UnityEngine;
using System.Collections;

public class UserEnvironmentBuilder : MonoBehaviour {

    private Object ghostedItemCursor;
    private const float wallSize = 4;

    void Start() {
        ghostedItemCursor = MonoBehaviour.Instantiate(Resources.Load("Prefabs/WallCursor"));
    }

    void Update() {
        UpdateCursorPosition();

        if (Input.GetMouseButtonDown(0)) { //left mouse clicked
            PlaceWall(MousePositionToGroundPosition());
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

    private static void PlaceWall(Vector3 position) {
        var wallLocation = PositionToGridPosition(position, wallSize);
        MonoBehaviour.Instantiate(Resources.Load("Prefabs/Wall"), wallLocation, Quaternion.identity);
    }

    void OnDestroy() {
        GameObject.Destroy(ghostedItemCursor);
    }
}
