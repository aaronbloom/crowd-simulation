using System;
using UnityEngine;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    internal class ToiletProcess : ThoughtProcess {

        private readonly Boid owner;
        private readonly Need ownerDesire;
        private const int SatisfactionRate = 20;

        public ToiletProcess(Boid boid, Need toSatisfy) {
            owner = boid;
            ownerDesire = toSatisfy;
            processList.Add((Action)navigateToToilet);
            processList.Add((Action)continueWalkToToilet);
            processList.Add((Action)reachToilet);
            processList.Add((Action)pee);
        }

        private void navigateToToilet() {
            GoalSeekingBehaviour gsb = new GoalSeekingBehaviour(owner);
            if (owner.Properties.Gender == Gender.MALE) {
                gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.MaleToilets);
            } else if (owner.Properties.Gender == Gender.FEMALE) {
                gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.FemaleToilets);
            } else {
                Debug.LogError("Toilet Gender Unavailable");
            }

            owner.behaviour = gsb;
            NextStep();
        }

        private void continueWalkToToilet() {
            if (owner.behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        private void reachToilet() {
            owner.Statistics.LogToiletBreak();
            NextStep();
        }

        private void pee()
        {
            ownerDesire.SatisfyByValue(SatisfactionRate);
        }

    }
}
