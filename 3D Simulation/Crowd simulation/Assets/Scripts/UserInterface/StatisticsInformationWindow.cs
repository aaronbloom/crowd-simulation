using UnityEngine;

namespace Assets.Scripts.UserInterface {
    internal class StatisticsInformationWindow {

        private const string BoidInformationPanelName = "StatisticsInformationPanel";
        private readonly GameObject statsInformationWindow;

        public StatisticsInformationWindow() {
            statsInformationWindow = GameObject.Find(BoidInformationPanelName);
            UserInterfaceController.HideMenu(statsInformationWindow);
        }

        public void Show() {
            UserInterfaceController.ShowMenu(statsInformationWindow);
        }

        public void Hide() {
            UserInterfaceController.HideMenu(statsInformationWindow);
        }

        public void Update() {

        }
    }
}
