using UnityEngine;

namespace Assets.Scripts.UserInterface {
    internal class StatisticsInformationWindow {

        private const string BoidInformationPanelName = "StatisticsInformationPanel";
        private readonly GameObject statsInformationWindow;

        public StatisticsInformationWindow() {
            statsInformationWindow = GameObject.Find(BoidInformationPanelName);
            UserInterfaceController.HideMenu(statsInformationWindow);
        }

        /// <summary>
        /// Shows statistics information window
        /// </summary>
        public void Show() {
            UserInterfaceController.ShowMenu(statsInformationWindow);
        }

        /// <summary>
        /// Hides statisitcs information window
        /// </summary>
        public void Hide() {
            UserInterfaceController.HideMenu(statsInformationWindow);
        }

        public void Update() {

        }
    }
}
