using UnityEngine;
using System.Collections;

public class UserEnvironmentBuilder : MonoBehaviour {

    void Update() {
        if (Input.GetMouseButtonDown(0)) { //left clicked
            Plane plane = new Plane(Vector3.up, Vector3.zero); // floor
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float distance;
            if (plane.Raycast(ray, out distance)) {
                Vector3 groundPoint = ray.GetPoint(distance);
                PlaceWall(groundPoint);
            }
        }
    }

    private Vector3 PositionToGridPosition(Vector3 position, float objectSize) {
        Vector3 gridPosition = position;
        gridPosition -= Vector3.one * (objectSize / 2);
        gridPosition /= objectSize;
        gridPosition = new Vector3(Mathf.Round(gridPosition.x), Mathf.Round(gridPosition.y), Mathf.Round(gridPosition.z));
        gridPosition *= objectSize;
        gridPosition += Vector3.one * (objectSize / 2);
        return gridPosition;
    }

    private void PlaceWall(Vector3 position) {
        float wallSize = 4;
        Vector3 wallLocation = PositionToGridPosition(position, wallSize);
        wallLocation.y = wallSize / 2;
        MonoBehaviour.Instantiate(Resources.Load("Prefabs/Wall"), wallLocation, Quaternion.identity);
    }
}
