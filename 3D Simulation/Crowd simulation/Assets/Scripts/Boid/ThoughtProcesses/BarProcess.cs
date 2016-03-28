using System;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    internal class BarProcess : ThoughtProcess {

        private const int SatisfactionMultiplier = 3;

        private readonly Boid owner;
        private readonly Need ownerDesire;

        private Goal currentGoal;

        public BarProcess(Boid boid, Need toSatisfy) {
            owner = boid;
            ownerDesire = toSatisfy;
            ProcessList.Add(navigateToBar);
            ProcessList.Add(continueWalkToBar);
            ProcessList.Add(reachBar);
            ProcessList.Add(drink);
        }

        private void navigateToBar() {
            GoalSeekingBehaviour gsb = new GoalSeekingBehaviour(owner);
            currentGoal = gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Bars);
            owner.Behaviour = gsb;
            NextStep();
        }

        private void continueWalkToBar() {
            if (owner.Behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        private void reachBar() {
            owner.Statistics.LogDrinkBought();
            NextStep();
        }

        private void drink()
        {
            ownerDesire.SatisfyByValue((int) owner.Properties.TraitProperties.DrinkNeedRate* SatisfactionMultiplier);
            owner.LookAt(currentGoal.GameObject.transform.position);
        }
    }
}
