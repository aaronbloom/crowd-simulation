using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface {
    class AnalysisInterface {
        private readonly Vector3 chartPosition = new Vector3(100, -100, 100);
        private  readonly Vector3 barChartPositionOffset = new Vector3(-0.5f, -0.5f, 0);
        private GameObject barChartObject;
        private BarChart barChartData;
        private ChartFrame barChartFrame;
        private GameObject chartBackgroundPlane;
        public StatisticsInformationWindow statisticsInformationWindow { get; private set; }

        public AnalysisInterface() {
            statisticsInformationWindow = new StatisticsInformationWindow();
        }

        private void AddCharts() {
            chartBackgroundPlane = BootStrapper.Initialise("Charts/ChartBackground") as GameObject;
            barChartObject = BootStrapper.Initialise("Charts/BarChart_Standard") as GameObject;
            barChartData = barChartObject.GetComponentInChildren<BarChart>();
            barChartFrame = barChartObject.GetComponentInChildren<ChartFrame>();
        }

        public void PopulateChart() {
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

        public void ViewChart() {
            BootStrapper.CameraManager.SwitchToStatsCamera(chartPosition, Vector3.forward);
        }

        public void HideChart() {
            BootStrapper.CameraManager.SwitchToRTSCamera();
        }

        public void SetStatisticsValues() {
            Text DistanceCoveredAverageText = GameObject.Find("DistanceCoveredAverageText").GetComponent<Text>();
            Text DistanceCoveredMinText = GameObject.Find("DistanceCoveredMinText").GetComponent<Text>();
            Text DistanceCoveredMaxText = GameObject.Find("DistanceCoveredMaxText").GetComponent<Text>();
            Text DrinksBoughtTotalText = GameObject.Find("DrinksBoughtTotalText").GetComponent<Text>();
            Text StageViewingAverageText = GameObject.Find("StageViewingAverageText").GetComponent<Text>();
            Text StageViewingMinText = GameObject.Find("StageViewingMinText").GetComponent<Text>();
            Text StageViewingMaxText = GameObject.Find("StageViewingMaxText").GetComponent<Text>();

            DistanceCoveredAverageText.text = BootStrapper.BoidManager.Boids.Average(boid => boid.Statistics.DistanceCovered).ToString();
            DistanceCoveredMinText.text = BootStrapper.BoidManager.Boids.Min(boid => boid.Statistics.DistanceCovered).ToString();
            DistanceCoveredMaxText.text = BootStrapper.BoidManager.Boids.Max(boid => boid.Statistics.DistanceCovered).ToString();
            DrinksBoughtTotalText.text = BootStrapper.BoidManager.Boids.Sum(boid => boid.Statistics.DrinksBought).ToString();
            StageViewingAverageText.text = BootStrapper.BoidManager.Boids.Average(boid => boid.Statistics.StageWatchedAmount).ToString();
            StageViewingMaxText.text = BootStrapper.BoidManager.Boids.Max(boid => boid.Statistics.StageWatchedAmount).ToString();
            StageViewingMinText.text = BootStrapper.BoidManager.Boids.Min(boid => boid.Statistics.StageWatchedAmount).ToString();
        }
    }
}
