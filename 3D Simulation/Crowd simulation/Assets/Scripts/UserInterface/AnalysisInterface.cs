using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UserInterface {
    internal class AnalysisInterface {

        public const string Chartxlabel = "ChartXLabel";
        private const string Chartylabel = "ChartYLabel";
        private const string Chartytext = "ChartYText";
        private const string Distancecoveredaveragetext = "DistanceCoveredAverageText";
        public const string Distancecoveredmintext = "DistanceCoveredMinText";
        public const string Distancecoveredmaxtext = "DistanceCoveredMaxText";
        public const string Drinksboughttotaltext = "DrinksBoughtTotalText";
        public const string Stageviewingaveragetext = "StageViewingAverageText";
        public const string Stageviewingmintext = "StageViewingMinText";
        private const string Stageviewingmaxtext = "StageViewingMaxText";
        private const string ChartsChartbackground = "Charts/ChartBackground";
        private const string ChartsBarchartStandard = "Charts/BarChart_Standard";

        private static readonly Vector3 chartPosition = new Vector3(100, -100, 100);
        private static readonly Vector3 barChartPositionOffset = new Vector3(-0.5f, -0.5f, 0);
        private static GameObject barChartObject;

        public StatisticsInformationWindow StatisticsInformationWindow { get; private set; }
        private BarChart barChartData;
        private GameObject chartBackgroundPlane;

        public AnalysisInterface() {
            StatisticsInformationWindow = new StatisticsInformationWindow();
        }

        public void PopulateChart() {
            addCharts();
            barChartData.UpdateData(
                BootStrapper.BoidManager.Boids.Select(boid => boid.Statistics.DrinksBought)
                .Select(drinksBought => (float)drinksBought).OrderBy(drinksBought => drinksBought).ToArray());
            barChartObject.transform.position = chartPosition + barChartPositionOffset;
            chartBackgroundPlane.transform.position = chartPosition + Vector3.forward*2;

            TextMesh chartXLabel = GameObject.Find(Chartxlabel).GetComponent<TextMesh>();
            TextMesh chartYLabel = GameObject.Find(Chartylabel).GetComponent<TextMesh>();
            TextMesh chartYText = GameObject.Find(Chartytext).GetComponent<TextMesh>();

            chartXLabel.transform.position = chartPosition + Vector3.down*0.5f;
            chartYLabel.transform.position = chartPosition + Vector3.left*0.5f;
            chartYText.transform.position = chartPosition + Vector3.left*0.5f + Vector3.up*0.5f;
            chartYText.text = BootStrapper.BoidManager.Boids.Max(boid => boid.Statistics.DrinksBought).ToString();
        }

        public void ViewChart() {
            BootStrapper.CameraManager.SwitchToStatsCamera(chartPosition + Vector3.left*0.4f, Vector3.forward);
        }

        public void HideChart() {
            BootStrapper.CameraManager.ActivateRTSCamera();
        }

        public void SetStatisticsValues() {
            Text distanceCoveredAverageText = GameObject.Find(Distancecoveredaveragetext).GetComponent<Text>();
            Text distanceCoveredMinText = GameObject.Find(Distancecoveredmintext).GetComponent<Text>();
            Text distanceCoveredMaxText = GameObject.Find(Distancecoveredmaxtext).GetComponent<Text>();
            Text drinksBoughtTotalText = GameObject.Find(Drinksboughttotaltext).GetComponent<Text>();
            Text stageViewingAverageText = GameObject.Find(Stageviewingaveragetext).GetComponent<Text>();
            Text stageViewingMinText = GameObject.Find(Stageviewingmintext).GetComponent<Text>();
            Text stageViewingMaxText = GameObject.Find(Stageviewingmaxtext).GetComponent<Text>();

            distanceCoveredAverageText.text = ((int)BootStrapper.BoidManager.Boids.Average(boid => boid.Statistics.DistanceCovered)).ToString();
            distanceCoveredMinText.text = ((int)BootStrapper.BoidManager.Boids.Min(boid => boid.Statistics.DistanceCovered)).ToString();
            distanceCoveredMaxText.text = ((int)BootStrapper.BoidManager.Boids.Max(boid => boid.Statistics.DistanceCovered)).ToString();
            drinksBoughtTotalText.text = BootStrapper.BoidManager.Boids.Sum(boid => boid.Statistics.DrinksBought).ToString();
            stageViewingAverageText.text = ((int)BootStrapper.BoidManager.Boids.Average(boid => boid.Statistics.StageWatchedAmount)).ToString();
            stageViewingMaxText.text = BootStrapper.BoidManager.Boids.Max(boid => boid.Statistics.StageWatchedAmount).ToString();
            stageViewingMinText.text = BootStrapper.BoidManager.Boids.Min(boid => boid.Statistics.StageWatchedAmount).ToString();
        }

        private void addCharts() {
            chartBackgroundPlane = BootStrapper.Initialise(ChartsChartbackground) as GameObject;
            barChartObject = BootStrapper.Initialise(ChartsBarchartStandard) as GameObject;
            barChartData = barChartObject.GetComponentInChildren<BarChart>();
        }
    }
}
