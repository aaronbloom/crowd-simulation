using System.Linq;
using UnityEngine;

namespace Assets.Scripts.UserInterface {
    class AnalysisInterface {
        private readonly Vector3 chartPosition = new Vector3(100, -100, 100);
        private  readonly Vector3 barChartPositionOffset = new Vector3(-0.5f, -0.5f, 0);
        private GameObject barChartObject;
        private BarChart barChartData;
        private ChartFrame barChartFrame;
        private GameObject chartBackgroundPlane;

        public AnalysisInterface() {}

        private void AddCharts() {
            chartBackgroundPlane = BootStrapper.Initialise("Charts/ChartBackground") as GameObject;
            barChartObject = BootStrapper.Initialise("Charts/BarChart_Standard") as GameObject;
            barChartData = barChartObject.GetComponentInChildren<BarChart>();
            barChartFrame = barChartObject.GetComponentInChildren<ChartFrame>();
        }

        public void Populate() {
            AddCharts();
            barChartData.UpdateData(
                BootStrapper.BoidManager.Boids.Select(boid => boid.Statistics.DrinksBought)
                .Select(drinksBought => (float)drinksBought).OrderBy(drinksBought => (float)drinksBought).ToArray());
            barChartObject.transform.position = chartPosition + barChartPositionOffset;
            chartBackgroundPlane.transform.position = chartPosition + Vector3.forward*2;
            barChartData.mBarWidth = 0.4f;

            barChartData.UpdateForEditor = true;
            barChartFrame.UpdateForEditor = true;

            barChartFrame.mThickness = 0.002f;
            barChartFrame.MarginBottom = 0;
            barChartFrame.MarginLeft = 0;
            barChartFrame.MarginRight = 0.1f;
            barChartFrame.MarginTop = 0.1f;
            barChartFrame.col_max = 10;
            barChartFrame.row_max = 0;
        }

        public void View() {
            BootStrapper.CameraManager.SwitchToStatsCamera(chartPosition, Vector3.forward);
        }
    }
}
