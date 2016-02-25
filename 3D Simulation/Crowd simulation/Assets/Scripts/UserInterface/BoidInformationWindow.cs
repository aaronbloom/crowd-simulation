using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    class BoidInformationWindow {

        private const string BoidInformationPanelName = "BoidInformationPanel";
        private readonly GameObject boidInformationWindow;
        private readonly Color MaxHealthColor = Color.red;
        private readonly Color MinHealthColor = Color.green;
        private readonly Slider ThirstSlider;
        private readonly Slider ToiletSlider;
        private readonly Slider DanceSlider;

        private String currentNeed;

        private Boid.Boid currentBoid;
        private GameObject selectionGameObject;

        public BoidInformationWindow() {
            boidInformationWindow = GameObject.Find(BoidInformationPanelName);
            

            ThirstSlider = GameObject.Find("ThirstSlider").GetComponent<Slider>();
            ToiletSlider = GameObject.Find("ToiletSlider").GetComponent<Slider>();
            DanceSlider = GameObject.Find("DanceSlider").GetComponent<Slider>();

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
                float totalNeed = currentBoid.Thirst + currentBoid.ToiletNeed + currentBoid.DanceNeed;
                setSliderValue(ThirstSlider, currentBoid.Thirst / totalNeed);
                setSliderValue(ToiletSlider, currentBoid.ToiletNeed / totalNeed);
                setSliderValue(DanceSlider, currentBoid.DanceNeed / totalNeed);
                GameObject.Find("BoidNameText").GetComponent<Text>().text = "Name: " + currentBoid.Properties.HumanName;
                GameObject.Find("CurrentNeedText").GetComponent<Text>().text = "Current need: " + currentBoid.CurrentNeed;
            }
        }

        private void setSliderValue(Slider slider, float value) {
            slider.value = value;
            InterpolateColour(slider);
        }

        private void InterpolateColour(Slider slider) {
            Image fill = slider.GetComponentsInChildren<Image>().FirstOrDefault(t => t.name == "Fill");
            fill.color = Color.Lerp(MinHealthColor, MaxHealthColor, slider.value/1);
        }

        private void UpdateSelectionGameObject() {
            if (selectionGameObject == null || currentNeed != currentBoid.CurrentNeed) {
                Object.Destroy(selectionGameObject);
                selectionGameObject = BootStrapper.Initialise(currentBoid.CurrentNeed) as GameObject;
                currentNeed = currentBoid.CurrentNeed;
            }
            Vector3 currentBoidPosition = currentBoid.transform.position;
            selectionGameObject.transform.position = currentBoidPosition + new Vector3(0, 4f, 0);
            selectionGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            selectionGameObject.transform.Rotate(Vector3.down, 100 * Time.deltaTime);
        }
    }
}
w