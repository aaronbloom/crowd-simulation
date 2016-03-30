namespace Assets.Scripts.Boid {

    /// <summary>
    /// Boid statistics class
    /// </summary>
    public class BoidStatistics {

        public float DistanceCovered { get; private set; }
        public int StageWatchedAmount { get; private set; }
        public int DrinksBought { get; private set; }
        public int ToiletBreaks { get; private set; }

        /// <summary>
        /// Boid statistics constuctor
        /// </summary>
        public BoidStatistics() {
            DistanceCovered = 0;
            StageWatchedAmount = 0;
            DrinksBought = 0;
            ToiletBreaks = 0;
        }

        /// <summary>
        /// Takes a distance travelled and logs it to a sum total
        /// </summary>
        /// <param name="differenceInDistance">Distance travelled</param>
        public void LogDistance(float differenceInDistance) {
            DistanceCovered += differenceInDistance;
        }

        /// <summary>
        /// Log one instance of stage viewing
        /// </summary>
        public void LogWatchingStage() {
            StageWatchedAmount++;
        }

        /// <summary>
        /// Log one drink bought
        /// </summary>
        public void LogDrinkBought() {
            DrinksBought++;
        }

        /// <summary>
        /// Log one toilet visit
        /// </summary>
        public void LogToiletBreak() {
            ToiletBreaks++;
        }
    }
}
