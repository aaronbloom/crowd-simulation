using System;
using UnityEngine;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    internal class ToiletProcess : ThoughtProcess {

        private const int SatisfactionRate = 20;

        private readonly Boid owner;
        private readonly Need ownerDesire;

        public ToiletProcess(Boid boid, Need toSatisfy) {
            owner = boid;
            ownerDesire = toSatisfy;
            ProcessList.Add(navigateToToilet);
            ProcessList.Add(continueWalkToToilet);
            ProcessList.Add(reachToilet);
            ProcessList.Add(pee);
        }

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

        private void continueWalkToToilet() {
            if (owner.Behaviour.BehaviourComplete) {
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
