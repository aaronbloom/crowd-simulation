using System;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {

    /// <summary>
    /// This class runs the process flow to satisfy the Drink Need
    /// </summary>
    internal class BarProcess : ThoughtProcess {

        private const int SatisfactionMultiplier = 3;

        private readonly Boid owner;
        private readonly Need ownerDesire;

        private Goal currentGoal;

        /// <summary>
        /// Creates new Bar Process
        /// </summary>
        /// <param name="boid">The boid this process will control</param>
        /// <param name="toSatisfy">The need this process will satisfy</param>
        public BarProcess(Boid boid, Need toSatisfy) {
            owner = boid;
            ownerDesire = toSatisfy;
            ProcessList.Add(navigateToBar);
            ProcessList.Add(continueWalkToBar);
            ProcessList.Add(reachBar);
            ProcessList.Add(drink);
        }

        /// <summary>
        /// Begins the boid walk to the bar
        /// </summary>
        private void navigateToBar() {
            GoalSeekingBehaviour gsb = new GoalSeekingBehaviour(owner);
            currentGoal = gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Bars);
            owner.Behaviour = gsb;
            NextStep();
        }

        /// <summary>
        /// Continues the boid walk to the bar
        /// </summary>
        private void continueWalkToBar() {
            if (owner.Behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        /// <summary>
        /// logs that the bar has been reached
        /// </summary>
        private void reachBar() {
            owner.Statistics.LogDrinkBought();
            NextStep();
        }

        /// <summary>
        /// slowly satisfied need
        /// </summary>
        private void drink()
        {
            ownerDesire.SatisfyByValue((int) owner.Properties.TraitProperties.DrinkNeedRate* SatisfactionMultiplier);
            owner.LookAt(currentGoal.GameObject.transform.position);
        }
    }
}
