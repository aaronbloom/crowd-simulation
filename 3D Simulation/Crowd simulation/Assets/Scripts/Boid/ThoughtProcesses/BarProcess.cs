using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    class BarProcess : ThoughtProcess {

        private Boid owner;
        private Need ownerDesire;
        private int satisfactionMultiplier = 3;
        private Goal currentGoal;

        public BarProcess(Boid boid, Need toSatisfy) : base() {
            owner = boid;
            ownerDesire = toSatisfy;
            processList.Add((Action)navigateToBar);
            processList.Add((Action)continueWalkToBar);
            processList.Add((Action)reachBar);
            processList.Add((Action)drink);
        }

        private void navigateToBar() {
            GoalSeekingBehaviour gsb = new GoalSeekingBehaviour(owner);
            currentGoal = gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Bars);
            owner.behaviour = gsb;
            NextStep();
        }

        private void continueWalkToBar() {
            if (owner.behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        private void reachBar() {
            owner.Statistics.LogDrinkBought();
            NextStep();
        }

        private void drink()
        {
            ownerDesire.SatisfyByValue((int) owner.Properties.DemographicProperties.DrinkNeedRate* satisfactionMultiplier);
            owner.LookAt(currentGoal.GameObject.transform.position);
        }
    }
}
