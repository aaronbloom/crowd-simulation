using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Boid {
    public class BoidStatistics {
        public float DistanceCovered { get; private set; }
        public int StageWatchedAmount { get; private set; }
        public int DrinksBought { get; private set; }

        public BoidStatistics() {
            DistanceCovered = 0;
            StageWatchedAmount = 0;
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
    }
}
