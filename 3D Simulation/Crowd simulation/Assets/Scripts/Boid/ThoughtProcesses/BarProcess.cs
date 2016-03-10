using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    class BarProcess : ThoughtProcess {
        private Boid owner;
        private Need ownerDesire;

        public BarProcess(Boid boid, Need toSatisfy) : base() {
            owner = boid;
            ownerDesire = toSatisfy;
            processList.Add((Action)navigateToBar);
            processList.Add((Action)continueWalkToBar);
            processList.Add((Action)reachBar);
        }

        private void navigateToBar() {
            GoalSeekingBehaviour gsb = new GoalSeekingBehaviour(owner);
            gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Bars);
            owner.behaviour = gsb;
            NextStep();
        }

        private void continueWalkToBar() {
            if (owner.behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        private void reachBar() {
            ownerDesire.Satisfy();
            owner.Statistics.LogDrinkBought();
            NextStep();
        }
    }
}
