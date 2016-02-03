using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    public class UserWorldBuilder {

        private WorldObject ghostedItemCursor;
        private readonly Material cursorMaterial;
        private const float cursorSize = 4;
        private string currentItem;
        private Vector3 startPlacement;
        private bool startedPlacement = false;

        public UserWorldBuilder() {
            cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
        }

        public void StartPlaceWorldObject() {
            if (currentItem != null) {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) { //is mouse pointer not over a menu ui
                    startPlacement = MousePositionToGroundPosition();
                    startedPlacement = true;
                }
            }
        }

        public void EndPlaceWorldObject() {
            if (currentItem != null) {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) { //is mouse pointer not over a menu ui
                    if (startedPlacement) {
                        PlaceLine(startPlacement, MousePositionToGroundPosition());
                    }
                }
            }
            startedPlacement = false;
        }

        public void SetCurrentPlacementObject(string objectName) {
            currentItem = objectName;
            Destroy();
            ghostedItemCursor = WorldObject.Initialise(DetermineObject(currentItem), MousePositionToGroundPosition());
            ghostedItemCursor.GameObject.GetComponent<Renderer>().material = cursorMaterial;
        }

        public void UpdateCursorPosition() {
            if (ghostedItemCursor != null) {
                ghostedItemCursor.GameObject.transform.position
                    = Environment.Environment.PositionToGridPosition(MousePositionToGroundPosition(), cursorSize);
            }
        }

        public void Destroy() {
            if (ghostedItemCursor != null) {
                Object.Destroy(ghostedItemCursor.GameObject);
            }
        }

        private void PlaceLine(Vector3 start, Vector3 end) {
            Vector3 step;
            var xDiff = start.x - end.x;
            var zDiff = start.z - end.z;
            float largerDiff;
            if (Mathf.Abs(xDiff) > Mathf.Abs(zDiff)) {
                step = new Vector3(-Mathf.Sign(xDiff), 0, 0);
                largerDiff = Mathf.Abs(xDiff);
            } else {
                step = new Vector3(0, 0, -Mathf.Sign(zDiff));
                largerDiff = Mathf.Abs(zDiff);
            }
            Vector3 position = start;
            for (int i = 0; i <= largerDiff; i++) {
                position += step;
                BootStrapper.EnvironmentManager.CurrentEnvironment.Place(DetermineObject(currentItem), position);
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
    }
}
