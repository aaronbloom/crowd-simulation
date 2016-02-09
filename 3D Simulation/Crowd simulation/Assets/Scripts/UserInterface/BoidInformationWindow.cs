using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface {
    class BoidInformationWindow {
        private readonly GameObject boidInformationWindow;
        private Boid.Boid currentBoid;
        private const string BoidInformationWindowName = "BoidInformationWindow";
        private GameObject selectionGameObject;

        public BoidInformationWindow() {
            boidInformationWindow = GameObject.Find(BoidInformationWindowName);
            UserInterfaceController.HideMenu(boidInformationWindow);
        }

        public void FindBoid(Vector3 screenPosition) {
            RaycastHit raycastHit;
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out raycastHit, 100)) {
                if (raycastHit.transform.tag == "Boid") {
                    UserInterfaceController.ShowMenu(boidInformationWindow);
                    currentBoid = raycastHit.transform.GetComponent<Boid.Boid>();
                } else {
                    currentBoid = null;
                    Object.Destroy(selectionGameObject);
                    selectionGameObject = null;
                    UserInterfaceController.HideMenu(boidInformationWindow);
                }
            }
        }

        public void Update() {
            if (currentBoid != null) {
                UpdateSelectionGameObject();
                GameObject.Find("ThirstText").GetComponent<Text>().text = "Thirst: " + currentBoid.Thirst;
                GameObject.Find("ToiletNeedText").GetComponent<Text>().text = "Toilet need: " + currentBoid.ToiletNeed;
                GameObject.Find("DanceNeedText").GetComponent<Text>().text = "Dance need: " + currentBoid.DanceNeed;
            }
        }

        private void UpdateSelectionGameObject() {
            if (selectionGameObject == null) {
                selectionGameObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            }
            Vector3 currentBoidPosition = currentBoid.transform.position;
            selectionGameObject.transform.position = currentBoidPosition + new Vector3(0, 4f, 0);
            selectionGameObject.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            selectionGameObject.transform.Rotate(Vector3.left + Vector3.down, 100 * Time.deltaTime);
        }
    }
}
