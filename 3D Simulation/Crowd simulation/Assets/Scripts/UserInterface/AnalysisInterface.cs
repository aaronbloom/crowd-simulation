using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface {
    internal class AnalysisInterface {
        private readonly Vector3 chartPosition = new Vector3(100, -100, 100);
        private  readonly Vector3 barChartPositionOffset = new Vector3(-0.5f, -0.5f, 0);
        private GameObject barChartObject;
        private BarChart barChartData;
        private GameObject chartBackgroundPlane;
        public StatisticsInformationWindow statisticsInformationWindow { get; private set; }

        public AnalysisInterface() {
            statisticsInformationWindow = new StatisticsInformationWindow();
        }

        private void AddCharts() {
            chartBackgroundPlane = BootStrapper.Initialise("Charts/ChartBackground") as GameObject;
            barChartObject = BootStrapper.Initialise("Charts/BarChart_Standard") as GameObject;
            barChartData = barChartObject.GetComponentInChildren<BarChart>();
        }

        public void PopulateChart() {
            AddCharts();
            barChartData.UpdateData(
                BootStrapper.BoidManager.Boids.Select(boid => boid.Statistics.DrinksBought)
                .Select(drinksBought => (float)drinksBought).OrderBy(drinksBought => (float)drinksBought).ToArray());
            barChartObject.transform.position = chartPosition + barChartPositionOffset;
            chartBackgroundPlane.transform.position = chartPosition + Vector3.forward*2;

            TextMesh chartXLabel = GameObject.Find("ChartXLabel").GetComponent<TextMesh>();
            TextMesh chartYLabel = GameObject.Find("ChartYLabel").GetComponent<TextMesh>();
            TextMesh chartYText = GameObject.Find("ChartYText").GetComponent<TextMesh>();

            chartXLabel.transform.position = chartPosition + Vector3.down*0.5f;
            chartYLabel.transform.position = chartPosition + Vector3.left*0.5f;
            chartYText.transform.position = chartPosition + Vector3.left*0.5f + Vector3.up*0.5f;
            chartYText.text = ((int) BootStrapper.BoidManager.Boids.Max(boid => boid.Statistics.DrinksBought)).ToString();
        }

        public void ViewChart() {
            BootStrapper.CameraManager.SwitchToStatsCamera(chartPosition + Vector3.left*0.4f, Vector3.forward);
        }

        public void HideChart() {
            BootStrapper.CameraManager.ActivateRTSCamera();
        }

        public void SetStatisticsValues() {
            Text DistanceCoveredAverageText = GameObject.Find("DistanceCoveredAverageText").GetComponent<Text>();
            Text DistanceCoveredMinText = GameObject.Find("DistanceCoveredMinText").GetComponent<Text>();
            Text DistanceCoveredMaxText = GameObject.Find("DistanceCoveredMaxText").GetComponent<Text>();
            Text DrinksBoughtTotalText = GameObject.Find("DrinksBoughtTotalText").GetComponent<Text>();
            Text StageViewingAverageText = GameObject.Find("StageViewingAverageText").GetComponent<Text>();
            Text StageViewingMinText = GameObject.Find("StageViewingMinText").GetComponent<Text>();
            Text StageViewingMaxText = GameObject.Find("StageViewingMaxText").GetComponent<Text>();

            DistanceCoveredAverageText.text = ((int)BootStrapper.BoidManager.Boids.Average(boid => boid.Statistics.DistanceCovered)).ToString();
            DistanceCoveredMinText.text = ((int)BootStrapper.BoidManager.Boids.Min(boid => boid.Statistics.DistanceCovered)).ToString();
            DistanceCoveredMaxText.text = ((int)BootStrapper.BoidManager.Boids.Max(boid => boid.Statistics.DistanceCovered)).ToString();
            DrinksBoughtTotalText.text = BootStrapper.BoidManager.Boids.Sum(boid => boid.Statistics.DrinksBought).ToString();
            StageViewingAverageText.text = ((int)BootStrapper.BoidManager.Boids.Average(boid => boid.Statistics.StageWatchedAmount)).ToString();
            StageViewingMaxText.text = BootStrapper.BoidManager.Boids.Max(boid => boid.Statistics.StageWatchedAmount).ToString();
            StageViewingMinText.text = BootStrapper.BoidManager.Boids.Min(boid => boid.Statistics.StageWatchedAmount).ToString();
        }
    }
}
