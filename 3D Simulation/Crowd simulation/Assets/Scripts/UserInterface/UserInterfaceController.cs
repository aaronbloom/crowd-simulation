using Assets.Scripts.Environment;
using UnityEngine;
using Assets.Scripts.Boid;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface
{
    public class UserInterfaceController : MonoBehaviour {

        private GameObject mainMenu;
        private GameObject setupMenu;
        private GameObject environmentBuilderMenu;
        private GameObject simulationMenu;
        private BoidInformationWindow boidInformationWindow;
        private UserWorldBuilder userWorldBuilder;

        void Awake() {
            mainMenu = GameObject.Find("MainMenu");
            setupMenu = GameObject.Find("SetupMenu");
            environmentBuilderMenu = GameObject.Find("EnvironmentBuilderMenu");
            simulationMenu = GameObject.Find("SimulationMenu");
            boidInformationWindow = new BoidInformationWindow();
        }

        void Start () {
            ShowMenu(mainMenu);
            HideMenu(setupMenu);
            HideMenu(environmentBuilderMenu);
            HideMenu(simulationMenu);
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

            if (Input.GetMouseButtonDown(0)) { //left mouse clicked
                boidInformationWindow.FindBoid(Input.mousePosition);
            }

            boidInformationWindow.Update();
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
            GameObject.Find("Bootstrapper").GetComponent<BootStrapper>().StopSimulation();
        }

        public void SetCurrentPlacementObject(string objectName) {
            userWorldBuilder.SetCurrentPlacementObject(objectName);
        }

        public static void HideMenu(GameObject menu) {
            menu.SetActive(false);
            CanvasGroup canvasGroup = menu.GetComponentInChildren<CanvasGroup>();
            if (canvasGroup != null) {
                canvasGroup.blocksRaycasts = false;
            }
        }

        public static void ShowMenu(GameObject menu) {
            menu.SetActive(true);
            CanvasGroup canvasGroup = menu.GetComponentInChildren<CanvasGroup>();
            if (canvasGroup != null) {
                canvasGroup.blocksRaycasts = true;
            }
        }
    }
}
