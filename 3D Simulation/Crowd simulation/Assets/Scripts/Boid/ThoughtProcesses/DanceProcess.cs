using System;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid.ThoughtProcesses {

    /// <summary>
    /// This class runs the process flow to satisfy the Dance Need
    /// </summary>
    internal class DanceProcess : ThoughtProcess {

        private readonly Boid owner;
        private readonly Need ownerDesire;

        private Goal currentGoal;

        /// <summary>
        /// Creates a new Dance process
        /// </summary>
        /// <param name="boid">The boid this process will control</param>
        /// <param name="toSatisfy">The need this process will satisfy</param>
        public DanceProcess(Boid boid, Need toSatisfy) {
            owner = boid;
            ownerDesire = toSatisfy;
            ProcessList.Add(navigateToStage);
            ProcessList.Add(continueWalkToStage);
            ProcessList.Add(reachStage);
        }

        /// <summary>
        /// Begins the boid walk to the stage
        /// </summary>
        private void navigateToStage() {
            GoalSeekingBehaviour behaviour = new LineOfSightGoalSeekingBehaviour(owner);
            this.currentGoal = behaviour.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Stages);
            owner.Behaviour = behaviour;
            NextStep();
        }

        /// <summary>
        /// Continues the boid walk to the bar
        /// </summary>
        private void continueWalkToStage() {
            if (owner.Behaviour.BehaviourComplete) {
                NextStep();
            }
        }

        /// <summary>
        /// slowly satisfied need
        /// </summary>
        private void reachStage() {
            //satisfy slowly
            ownerDesire.SatisfyByValue((int) owner.Properties.TraitProperties.DanceNeedRate + 1);
            owner.Statistics.LogWatchingStage();
            owner.LookAt(currentGoal.GameObject.transform.position);
            //NextStep();
        }
    }
}
