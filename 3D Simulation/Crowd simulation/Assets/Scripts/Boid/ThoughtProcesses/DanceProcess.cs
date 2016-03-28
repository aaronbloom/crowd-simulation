using System;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    internal class DanceProcess : ThoughtProcess {

        private readonly Boid owner;
        private readonly Need ownerDesire;

        private Goal currentGoal;

        public DanceProcess(Boid boid, Need toSatisfy) {
            owner = boid;
            ownerDesire = toSatisfy;
            ProcessList.Add(navigateToStage);
            ProcessList.Add(continueWalkToStage);
            ProcessList.Add(reachStage);
        }

        private void navigateToStage() {
            GoalSeekingBehaviour behaviour = new LineOfSightGoalSeekingBehaviour(owner);
            this.currentGoal = behaviour.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Stages);
            owner.Behaviour = behaviour;
            NextStep();
        }

        private void continueWalkToStage() {
            if (owner.Behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        private void reachStage() {
            //satisfy slowly
            ownerDesire.SatisfyByValue((int) owner.Properties.TraitProperties.DanceNeedRate + 1);
            owner.Statistics.LogWatchingStage();
            owner.LookAt(currentGoal.GameObject.transform.position);
            //NextStep();
        }
    }
}
