﻿using Assets.Scripts.Environment;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
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
        private AnalysisInterface analysisInterface;

        void Awake() {
            mainMenu = GameObject.Find("MainMenu");
            setupMenu = GameObject.Find("SetupMenu");
            environmentBuilderMenu = GameObject.Find("EnvironmentBuilderMenu");
            simulationMenu = GameObject.Find("SimulationMenu");
            demographicMenu = GameObject.Find("DemographicMenu");
            analysisMenu = GameObject.Find("AnalysisMenu");
            boidInformationWindow = new BoidInformationWindow();
            analysisInterface = new AnalysisInterface();
        }

        void Start () {
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
            int environmentHeight = 50;
            int environmentSize = setupMenu.GetComponent<MenuControlController>().EnvironmentSizeValue;
            Vector3 bounds = new Vector3(environmentSize, environmentHeight, environmentSize);
            EnvironmentManager.Shared().InitialiseEnvironment(bounds);
            HideMenu(setupMenu);
            ShowMenu(environmentBuilderMenu);
            userWorldBuilder = new UserWorldBuilder();
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
            analysisInterface.statisticsInformationWindow.Hide();
        }

        public void ShowDrinksBought() {
            analysisInterface.PopulateChart();
            analysisInterface.ViewChart();
            analysisInterface.statisticsInformationWindow.Hide();
        }

        public void ShowStatistics() {
            analysisInterface.statisticsInformationWindow.Show();
            analysisInterface.SetStatisticsValues();
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
