using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Boid {
    public class BoidStatistics {
        public float DistanceCovered { get; private set; }

        public BoidStatistics() {
            DistanceCovered = 0;
        }

        public void LogDistance(float differenceInDistance) {
            DistanceCovered += differenceInDistance;
        }
    }
}
