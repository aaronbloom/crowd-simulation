using System;
using UnityEngine;

namespace Assets.Scripts.Boid.ThoughtProcesses {

    /// <summary>
    /// This class runs the process flow to satisfy the Toilet Need
    /// </summary>
    internal class ToiletProcess : ThoughtProcess {

        private const int SatisfactionRate = 20;

        private readonly Boid owner;
        private readonly Need ownerDesire;

        /// <summary>
        /// Creates a new Toilet process
        /// </summary>
        /// <param name="boid">The boid this process will control</param>
        /// <param name="toSatisfy">The need this process will satisfy</param>
        public ToiletProcess(Boid boid, Need toSatisfy) {
            owner = boid;
            ownerDesire = toSatisfy;
            ProcessList.Add(navigateToToilet);
            ProcessList.Add(continueWalkToToilet);
            ProcessList.Add(reachToilet);
            ProcessList.Add(pee);
        }

        /// <summary>
        /// Begins the boid walk to the toilet
        /// </summary>
        private void navigateToToilet() {
            GoalSeekingBehaviour gsb = new GoalSeekingBehaviour(owner);
            if (owner.Properties.Gender == Gender.Male) {
                gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.MaleToilets);
            } else if (owner.Properties.Gender == Gender.Female) {
                gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.FemaleToilets);
            } else {
                Debug.LogError("Toilet Gender Unavailable");
            }

            owner.Behaviour = gsb;
            NextStep();
        }

        /// <summary>
        /// Continues the boid walk to the toilet
        /// </summary>
        private void continueWalkToToilet() {
            if (owner.Behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        /// <summary>
        /// logs that the toilet has been reached
        /// </summary>
        private void reachToilet() {
            owner.Statistics.LogToiletBreak();
            NextStep();
        }

        /// <summary>
        /// slowly satisfied need
        /// </summary>
        private void pee()
        {
            ownerDesire.SatisfyByValue(SatisfactionRate);
        }

    }
}
