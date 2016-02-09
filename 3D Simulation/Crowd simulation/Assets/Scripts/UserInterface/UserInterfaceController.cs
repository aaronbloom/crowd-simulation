using Assets.Scripts.Environment;
using UnityEngine;

namespace Assets.Scripts.UserInterface
{
    public class UserInterfaceController : MonoBehaviour {

        private GameObject mainMenu;
        private GameObject setupMenu;
        private GameObject environmentBuilderMenu;
        private GameObject simulationMenu;
        private GameObject analysisMenu;
        private UserWorldBuilder userWorldBuilder;

        void Awake() {
            mainMenu = GameObject.Find("MainMenu");
            setupMenu = GameObject.Find("SetupMenu");
            environmentBuilderMenu = GameObject.Find("EnvironmentBuilderMenu");
            simulationMenu = GameObject.Find("SimulationMenu");
            analysisMenu = GameObject.Find("AnalysisMenu");
        }

        void Start () {
            ShowMenu(mainMenu);
            HideMenu(setupMenu);
            HideMenu(environmentBuilderMenu);
            HideMenu(simulationMenu);
            HideMenu(analysisMenu);
        }
	
        void Update () {
            if (userWorldBuilder != null) {
                userWorldBuilder.UpdateCursorPosition();
                if (Input.GetMouseButtonDown(0)) { //left mouse clicked
                    Debug.Log("Mouse down");
                    userWorldBuilder.StartPlaceWorldObject();
                }
                if (Input.GetMouseButtonUp(0)) {
                    Debug.Log("Mouse up");
                    userWorldBuilder.EndPlaceWorldObject();
                }
            }
        }

        public void NewSimulation() {
            HideMenu(mainMenu);
            ShowMenu(setupMenu);
        }

        public void StartEnvironmentBuilder() {
            int environmentHeight = 50;
            int environmentSize = setupMenu.GetComponent<MenuControlController>().EnvironmentSizeValue;
            Vector3 bounds = new Vector3(environmentSize, environmentHeight, environmentSize);
            EnvironmentManager.Shared().InitialiseEnvironment(bounds);
            HideMenu(setupMenu);
            ShowMenu(environmentBuilderMenu);
            userWorldBuilder = new UserWorldBuilder();
            BootStrapper.CameraController.LookAtEnvironmentCenter();
        }

        public void StartSimulation() {
            userWorldBuilder.DestroyCursors();
            userWorldBuilder = null;
            HideMenu(mainMenu);
            HideMenu(setupMenu);
            HideMenu(environmentBuilderMenu);
            int numberOfBoids = setupMenu.GetComponent<MenuControlController>().NumberOfBoidsValue;
            GameObject.Find("Bootstrapper").GetComponent<BootStrapper>().StartSimulation(numberOfBoids);
            ShowMenu(simulationMenu);
        }

        public void StopSimulation() {
            HideMenu(simulationMenu);
            ShowMenu(analysisMenu);
            GameObject.Find("Bootstrapper").GetComponent<BootStrapper>().StopSimulation();
        }

        public void SetCurrentPlacementObject(string objectName) {
            userWorldBuilder.SetCurrentPlacementObject(objectName);
        }

        private void HideMenu(GameObject menu) {
            menu.SetActive(false);
            CanvasGroup canvasGroup = menu.GetComponentInChildren<CanvasGroup>();
            if (canvasGroup != null) {
                canvasGroup.blocksRaycasts = false;
            }
        }

        private void ShowMenu(GameObject menu) {
            menu.SetActive(true);
            CanvasGroup canvasGroup = menu.GetComponentInChildren<CanvasGroup>();
            if (canvasGroup != null) {
                canvasGroup.blocksRaycasts = true;
            }
        }
    }
}
