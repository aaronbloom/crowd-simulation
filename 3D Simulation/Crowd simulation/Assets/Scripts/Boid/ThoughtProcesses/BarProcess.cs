using System;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    internal class BarProcess : ThoughtProcess {

        private readonly Boid owner;
        private readonly Need ownerDesire;
        private const int SatisfactionMultiplier = 3;
        private Goal currentGoal;

        public BarProcess(Boid boid, Need toSatisfy) {
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
            ownerDesire.SatisfyByValue((int) owner.Properties.DemographicProperties.DrinkNeedRate* SatisfactionMultiplier);
            owner.LookAt(currentGoal.GameObject.transform.position);
        }
    }
}
