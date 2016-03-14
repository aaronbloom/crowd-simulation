using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Boid.ThoughtProcesses {

    class ToiletProcess : ThoughtProcess {

        private Boid owner;
        private Need ownerDesire;
        private int satisfactionRate = 20;

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
            gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Toilets);
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
            ownerDesire.SatisfyByValue(satisfactionRate);
        }

    }
}
