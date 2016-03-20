using System;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    class DanceProcess : ThoughtProcess {
        private Boid owner;
        private Need ownerDesire;
        private Goal currentGoal;

        public DanceProcess(Boid boid, Need toSatisfy) : base() {
            owner = boid;
            ownerDesire = toSatisfy;
            processList.Add((Action)navigateToStage);
            processList.Add((Action)continueWalkToStage);
            processList.Add((Action)reachStage);
        }

        private void navigateToStage() {
            GoalSeekingBehaviour behaviour = new LineOfSightGoalSeekingBehaviour(owner);
            this.currentGoal = behaviour.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Stages);
            owner.behaviour = behaviour;
            NextStep();
        }

        private void continueWalkToStage() {
            if (owner.behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        private void reachStage() {
            //satisfy slowly
            ownerDesire.SatisfyByValue((int) owner.Properties.DemographicProperties.DanceNeedRate + 1);
            owner.Statistics.LogWatchingStage();
            owner.LookAt(currentGoal.GameObject.transform.position);
            //NextStep();
        }
    }
}
