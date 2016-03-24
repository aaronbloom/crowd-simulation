using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    internal class ToiletProcess : ThoughtProcess {

        private readonly Boid owner;
        private readonly Need ownerDesire;
        private const int SatisfactionRate = 20;

        public ToiletProcess(Boid boid, Need toSatisfy) : base() {
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
                UnityEngine.Debug.LogError("Toilet Gender Unavailable");
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
