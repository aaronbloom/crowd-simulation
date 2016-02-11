using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    class DanceProcess : ThoughtProcess {
        private Boid owner;
        private Need ownerDesire;

        public DanceProcess(Boid boid, Need toSatisfy) : base() {
            owner = boid;
            ownerDesire = toSatisfy;
            processList.Add((Action)navigateToStage);
            processList.Add((Action)continueWalkToStage);
            processList.Add((Action)reachStage);
        }

        private void navigateToStage() {
            GoalSeekingBehaviour gsb = new LineOfSightGoalSeekingBehaviour(owner, owner.viewingDistance, owner.minimumDistance, Stage.Name);
            gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Stages);
            owner.behaviour = gsb;
            NextStep();
        }

        private void continueWalkToStage() {
            if (owner.behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        private void reachStage() {
            //probably change this to some loiter behaviour
            //satisfy slowly
            ownerDesire.Satisfy(5);
            //NextStep();
        }
    }
}
