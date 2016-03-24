namespace Assets.Scripts.Boid {
    public class BoidStatistics {

        public float DistanceCovered { get; private set; }
        public int StageWatchedAmount { get; private set; }
        public int DrinksBought { get; private set; }
        public int ToiletBreaks { get; private set; }

        public BoidStatistics() {
            DistanceCovered = 0;
            StageWatchedAmount = 0;
            DrinksBought = 0;
            ToiletBreaks = 0;
        }

        public void LogDistance(float differenceInDistance) {
            DistanceCovered += differenceInDistance;
        }

        public void LogWatchingStage() {
            StageWatchedAmount++;
        }

        public void LogDrinkBought() {
            DrinksBought++;
        }

        public void LogToiletBreak() {
            ToiletBreaks++;
        }
    }
}
