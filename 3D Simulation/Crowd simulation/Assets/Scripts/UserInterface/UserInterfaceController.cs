﻿using Assets.Scripts.Environment;
using UnityEngine;
using Assets.Scripts.Boid;
using Assets.Scripts.Environment.Save;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface
{
    public class UserInterfaceController : MonoBehaviour {

        private const int LeftMouseButton = 0;
        private const int RightMouseButton = 1;

        private GameObject mainMenu;
        private GameObject setupMenu;
        private GameObject environmentBuilderMenu;
        private GameObject simulationMenu;
        private GameObject demographicMenu;
        private GameObject analysisMenu;
        private BoidInformationWindow boidInformationWindow;
        private UserWorldBuilder userWorldBuilder;

        void Awake() {
            mainMenu = GameObject.Find("MainMenu");
            setupMenu = GameObject.Find("SetupMenu");
            environmentBuilderMenu = GameObject.Find("EnvironmentBuilderMenu");
            simulationMenu = GameObject.Find("SimulationMenu");
            demographicMenu = GameObject.Find("DemographicMenu");
            analysisMenu = GameObject.Find("AnalysisMenu");
            boidInformationWindow = new BoidInformationWindow();
        }

        void Start() {
            SetupMainMenu();
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
                if (Input.GetMouseButtonDown(RightMouseButton)) {
                    userWorldBuilder.DeleteWorldObject();
                }
            }

            if (Input.GetMouseButtonDown(LeftMouseButton)) {
                boidInformationWindow.FindBoid(Input.mousePosition);
            }

            boidInformationWindow.Update();
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
            //BootStrapper.CameraController.LookAtEnvironmentCenter();
        }

        public void DemographicSetup() {
            userWorldBuilder.Destroy();
            userWorldBuilder = null;
            HideMenu(environmentBuilderMenu);
            ShowMenu(demographicMenu);
        }

        public void StartSimulation() {
            HideMenu(mainMenu);
            HideMenu(setupMenu);
            HideMenu(environmentBuilderMenu);
            HideMenu(demographicMenu);
            MenuControlController menuControlController = setupMenu.GetComponent<MenuControlController>();
            int numberOfBoids = menuControlController.NumberOfBoidsValue;
            float genderBias = menuControlController.GenderBiasValue;
            GameObject.Find("Bootstrapper").GetComponent<BootStrapper>().StartSimulation(numberOfBoids, genderBias);
            ShowMenu(simulationMenu);
        }

        public void StopSimulation() {
            HideMenu(simulationMenu);
            ShowMenu(analysisMenu);
            GameObject.Find("Bootstrapper").GetComponent<BootStrapper>().StopSimulation();
        }

        public void GenerateHeatMap() {
            BootStrapper.BoidManager.DisplayHeatMap();
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

        public void SaveWorld() {
            BootStrapper.EnvironmentManager.CurrentEnvironment.SaveEnvironment();
        }

        public void LoadWorld(string worldFileName) {
            HideMenu(mainMenu);
            BootStrapper.EnvironmentManager.LoadEnvironmentFromFile(worldFileName);
            ShowMenu(environmentBuilderMenu);
            userWorldBuilder = new UserWorldBuilder();
        }

        private void SetupMainMenu() {
            int x = 0;
            int y = +90;
            int z = 0;

            for (int i = 0; i < SystemSaveFolder.AmountOfFilesWithNameInFolder("World"); i++) {
                var button = BootStrapper.Initialise("LoadSimulationButton") as GameObject;
                button.transform.parent = mainMenu.transform;
                var textItem = button.GetComponentInChildren<Text>();
                textItem.text = SystemSaveFolder.WorldSaveName + " (" + i + ")";
                button.GetComponent<Button>().onClick.AddListener(delegate { LoadWorld(textItem.text); });
                button.GetComponent<RectTransform>().localPosition = new Vector3(x,y,z);
                x += 150;
                if (x > 150) {
                    x = -150;
                    y -= 150;
                }
            }
            while (x <= 150)
            {
                var blank = BootStrapper.Initialise("Button Background") as GameObject;
                blank.transform.parent = mainMenu.transform;
                blank.GetComponent<RectTransform>().localPosition = new Vector3(x, y, z);
                x += 150;
            }
        }
    }
}
