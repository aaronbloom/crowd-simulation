using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.UserInterface {
    internal class BoidInformationWindow {

        private const string BoidInformationPanelName = "BoidInformationPanel";
        private const string ThirstSlider = "ThirstSlider";
        private const string ToiletSlider = "ToiletSlider";
        private const string DanceSlider = "DanceSlider";
        private const string BoidNameText = "BoidNameText";
        private const string BoidDemographicText = "BoidDemographicText";
        private const string CurrentNeedText = "CurrentNeedText";
        private const string Boid = "Boid";
        private const string Fill = "Fill";

        private readonly GameObject boidInformationWindow;
        private readonly Color maxHealthColor = Color.red;
        private readonly Color minHealthColor = Color.green;
        private readonly Slider thirstSlider;
        private readonly Slider toiletSlider;
        private readonly Slider danceSlider;

        private string currentNeed;
        private Boid.Boid currentBoid;
        private GameObject selectionGameObject;

        /// <summary>
        /// Creates new boid information window
        /// </summary>
        public BoidInformationWindow() {
            boidInformationWindow = GameObject.Find(BoidInformationPanelName);
            thirstSlider = GameObject.Find(ThirstSlider).GetComponent<Slider>();
            toiletSlider = GameObject.Find(ToiletSlider).GetComponent<Slider>();
            danceSlider = GameObject.Find(DanceSlider).GetComponent<Slider>();

            UserInterfaceController.HideMenu(boidInformationWindow);
        }

        /// <summary>
        /// Loads a boid into the boid information window if there is one below the cursor
        /// </summary>
        /// <param name="screenPosition"></param>
        public void FindBoid(Vector3 screenPosition) {
            RaycastHit raycastHit;
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(screenPosition);
            if (Physics.Raycast(ray, out raycastHit, 100)) {
                if (raycastHit.transform.tag == Boid) {
                    UserInterfaceController.ShowMenu(boidInformationWindow);
                    currentBoid = BootStrapper.BoidManager.FindBoid(raycastHit.transform.gameObject);
                } else {
                    currentBoid = null;
                    Object.Destroy(selectionGameObject);
                    selectionGameObject = null;
                    UserInterfaceController.HideMenu(boidInformationWindow);
                }
            }
        }

        /// <summary>
        /// Updates the boid information window
        /// </summary>
        public void Update() {
            if (currentBoid != null) {
                UpdateSelectionGameObject();
                float totalNeed = currentBoid.Thirst + currentBoid.ToiletNeed + currentBoid.DanceNeed;
                setSliderValue(thirstSlider, currentBoid.Thirst / totalNeed);
                setSliderValue(toiletSlider, currentBoid.ToiletNeed / totalNeed);
                setSliderValue(danceSlider, currentBoid.DanceNeed / totalNeed);
                GameObject.Find(BoidNameText).GetComponent<Text>().text = "Name: " + currentBoid.Properties.HumanName;
                GameObject.Find(BoidDemographicText).GetComponent<Text>().text = "Trait: " + currentBoid.Properties.TraitProperties;
                GameObject.Find(CurrentNeedText).GetComponent<Text>().text = "Current need: " + currentBoid.CurrentNeed;
            }
        }

        /// <summary>
        /// Sets the value of a slide in the boid information window
        /// </summary>
        /// <param name="slider">the slider to update</param>
        /// <param name="value">the value</param>
        private void setSliderValue(Slider slider, float value) {
            slider.value = value;
            InterpolateColour(slider);
        }

        /// <summary>
        /// Gives a smooth colour to the slider
        /// </summary>
        /// <param name="slider">the slider to give a colour to</param>
        private void InterpolateColour(Slider slider) {
            Image fill = slider.GetComponentsInChildren<Image>().FirstOrDefault(t => t.name == Fill);
            fill.color = Color.Lerp(minHealthColor, maxHealthColor, slider.value/1);
        }

        /// <summary>
        /// Update the floating boid status indicator
        /// </summary>
        private void UpdateSelectionGameObject() {
            if ((selectionGameObject == null) || (currentNeed != currentBoid.CurrentNeed)) {
                if (selectionGameObject != null) {
                    Object.Destroy(selectionGameObject);
                }
                selectionGameObject = BootStrapper.Initialise(currentBoid.CurrentNeed) as GameObject;
                currentNeed = currentBoid.CurrentNeed;
            }
            Vector3 currentBoidPosition = currentBoid.Position;
            selectionGameObject.transform.position = currentBoidPosition + new Vector3(0, 4f, 0);
            selectionGameObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            selectionGameObject.transform.Rotate(Vector3.down, 100 * Time.deltaTime);
        }
    }
}
