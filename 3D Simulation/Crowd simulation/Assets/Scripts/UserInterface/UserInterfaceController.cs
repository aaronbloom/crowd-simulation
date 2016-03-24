using Assets.Scripts.Environment;
using Assets.Scripts.Environment.Save;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface {
    public class UserInterfaceController : MonoBehaviour {

        private const int LeftMouseButton = 0;
        private const int Z = 0;
        private const int Offset = 150;
        private const string Mainmenu = "MainMenu";
        private const string Setupmenu = "SetupMenu";
        private const string Environmentbuildermenu = "EnvironmentBuilderMenu";
        private const string Simulationmenu = "SimulationMenu";
        private const string Demographicmenu = "DemographicMenu";
        private const string Analysismenu = "AnalysisMenu";

        private const string Demographicsetupbutton = "DemographicSetupButton";
        private const string Environmentbuilderinformation = "EnvironmentBuilderInformation";
        private const string Bootstrapper = "Bootstrapper";
        private const string FileName = "World";
        private const string PrefabName = "LoadSimulationButton";
        private const string ButtonBackground = "Button Background";

        private GameObject mainMenu;
        private GameObject setupMenu;
        private GameObject environmentBuilderMenu;
        private GameObject simulationMenu;
        private GameObject demographicMenu;
        private GameObject analysisMenu;
        private BoidInformationWindow boidInformationWindow;
        private UserWorldBuilder userWorldBuilder;
        private AnalysisInterface analysisInterface;

        void Awake() {
            mainMenu = GameObject.Find(Mainmenu);
            setupMenu = GameObject.Find(Setupmenu);
            environmentBuilderMenu = GameObject.Find(Environmentbuildermenu);
            simulationMenu = GameObject.Find(Simulationmenu);
            demographicMenu = GameObject.Find(Demographicmenu);
            analysisMenu = GameObject.Find(Analysismenu);
            analysisInterface = new AnalysisInterface();
        }

        void Start() {
            setupMainMenu();
            ShowMenu(mainMenu);
            HideMenu(setupMenu);
            HideMenu(environmentBuilderMenu);
            HideMenu(simulationMenu);
            HideMenu(demographicMenu);
            HideMenu(analysisMenu);
        }

        void Update () {
            if (userWorldBuilder != null) {
                userWorldBuilder.UpdateCursorPosition();
                if (Input.GetMouseButtonDown(LeftMouseButton)) {
                    userWorldBuilder.StartPlaceWorldObject();
                }
                if (Input.GetMouseButtonUp(LeftMouseButton)) {
                    userWorldBuilder.EndPlaceWorldObject();
                }

                //button only active when environment is valid
                var isValidWorld = EnvironmentManager.Shared().CurrentEnvironment.World.IsValidWorld();
                GameObject.Find(Demographicsetupbutton).GetComponent<Button>().interactable = isValidWorld;
                GameObject.Find(Environmentbuilderinformation).GetComponent<Text>().enabled = !isValidWorld;
            }

            if (boidInformationWindow != null) {
                if (Input.GetMouseButtonDown(LeftMouseButton)) {
                    boidInformationWindow.FindBoid(Input.mousePosition);
                }
                boidInformationWindow.Update();
            }

            if (Input.GetKey(KeyCode.Escape)) {
                BootStrapper.CameraManager.ActivateRTSCamera();
            }
        }

        public void NewSimulation() {
            HideMenu(mainMenu);
            ShowMenu(setupMenu);
        }

        public void StartEnvironmentBuilder() {
            EnvironmentManager.Shared().InitialiseEnvironment(setupMenu.GetComponent<MenuControlController>().EnvironmentSizeValue);
            HideMenu(setupMenu);
            ShowMenu(environmentBuilderMenu);
            userWorldBuilder = new UserWorldBuilder();
        }

        public void DemographicSetup() {
            if (EnvironmentManager.Shared().CurrentEnvironment.World.IsValidWorld()) {
                userWorldBuilder.Destroy();
                userWorldBuilder = null;
                HideMenu(environmentBuilderMenu);
                ShowMenu(demographicMenu);
            }
        }

        public void StartSimulation() {
            HideMenu(mainMenu);
            HideMenu(setupMenu);
            HideMenu(environmentBuilderMenu);
            HideMenu(demographicMenu);
            MenuControlController menuControlController = setupMenu.GetComponent<MenuControlController>();
            int numberOfBoids = menuControlController.NumberOfBoidsValue;
            float genderBias = menuControlController.GenderBiasValue;
            GameObject.Find(Bootstrapper).GetComponent<BootStrapper>().StartSimulation(numberOfBoids, genderBias);
            ShowMenu(simulationMenu);
            boidInformationWindow = new BoidInformationWindow();
        }

        public void StopSimulation() {
            boidInformationWindow = null;
            HideMenu(simulationMenu);
            ShowMenu(analysisMenu);
            GameObject.Find(Bootstrapper).GetComponent<BootStrapper>().StopSimulation();
        }

        public void BoidsEyeView() {
            BootStrapper.CameraManager.ActivateFirstPersonCamera();
            EventSystem.current.SetSelectedGameObject(null, null);
        }

        public void GenerateHeatMap() {
            BootStrapper.BoidManager.DisplayHeatMap();
            analysisInterface.StatisticsInformationWindow.Hide();
        }

        public void ShowDrinksBought() {
            analysisInterface.PopulateChart();
            analysisInterface.ViewChart();
            analysisInterface.StatisticsInformationWindow.Hide();
        }

        public void ShowStatistics() {
            analysisInterface.StatisticsInformationWindow.Show();
            analysisInterface.SetStatisticsValues();
        }

        public void SetCurrentPlacementObject(string objectName) {
            userWorldBuilder.SetCurrentPlacementObject(objectName);
        }

        public void EnableDeletionTool() {
            userWorldBuilder.EnableDeletionTool();
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

        public void SaveWorld() {
            BootStrapper.EnvironmentManager.CurrentEnvironment.SaveEnvironment();
        }

        public void LoadWorld(string worldFileName) {
            HideMenu(mainMenu);
            BootStrapper.EnvironmentManager.LoadEnvironmentFromFile(worldFileName);
            ShowMenu(environmentBuilderMenu);
            userWorldBuilder = new UserWorldBuilder();
        }

        private void setupMainMenu() {
            int x = 0;
            int y = 90;

            for (int i = 0; i < SystemSaveFolder.AmountOfFilesWithNameInFolder(FileName); i++) {
                var button = BootStrapper.Initialise(PrefabName) as GameObject;
                button.transform.SetParent(mainMenu.transform);
                var textItem = button.GetComponentInChildren<Text>();
                textItem.text = SystemSaveFolder.WorldSaveName + " (" + i + ")";
                button.GetComponent<Button>().onClick.AddListener(delegate { LoadWorld(textItem.text); });
                button.GetComponent<RectTransform>().localPosition = new Vector3(x,y,Z);
                x += Offset;
                if (x > Offset) {
                    x = -Offset;
                    y -= Offset;
                }
            }
            while (x <= Offset) {
                var blank = BootStrapper.Initialise(ButtonBackground) as GameObject;
                blank.transform.SetParent(mainMenu.transform);
                blank.GetComponent<RectTransform>().localPosition = new Vector3(x, y, Z);
                x += Offset;
            }
        }
    }
}
