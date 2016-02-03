using Assets.Scripts.Environment.World.Objects;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    public class UserWorldBuilder {

        private WorldObject ghostedItemCursor;
        private readonly Material cursorMaterial;
        private const float cursorSize = 4;
        private string currentItem;

        public UserWorldBuilder() {
            cursorMaterial = Resources.Load("Materials/Cursor", typeof(Material)) as Material;
        }

        public void PlaceWorldObject() {
            if (currentItem != null) {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)) { //is mouse pointer not over a menu ui
                    BootStrapper.EnvironmentManager.CurrentEnvironment.Place(DetermineObject(currentItem), MousePositionToGroundPosition());
                }
            }
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
