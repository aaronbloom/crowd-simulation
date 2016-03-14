using System;
using Assets.Scripts.Environment.Navigation;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    class DanceProcess : ThoughtProcess {
        private Boid owner;
        private Need ownerDesire;
        private Node goalNode;

        public DanceProcess(Boid boid, Need toSatisfy) : base() {
            owner = boid;
            ownerDesire = toSatisfy;
            processList.Add((Action)navigateToStage);
            processList.Add((Action)continueWalkToStage);
            processList.Add((Action)reachStage);
        }

        private void navigateToStage() {
            GoalSeekingBehaviour behaviour = new LineOfSightGoalSeekingBehaviour(owner);
            behaviour.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Stages);
            this.goalNode = behaviour.GoalNode;
            owner.behaviour = behaviour;
            NextStep();
        }

        private void continueWalkToStage() {
            if (owner.behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        private void reachStage() {
            WatchingBehaviour behaviour = new WatchingBehaviour(this.owner, this.goalNode);
            owner.behaviour = behaviour;
            //satisfy slowly
            ownerDesire.SatisfyByValue((int) owner.Properties.DemographicProperties.DanceNeedRate + 1);
            owner.Statistics.LogWatchingStage();
            //NextStep();
        }
    }
}
