using System;
using System.Collections.Generic;

namespace Assets.Scripts.Boid.ThoughtProcesses {

    /// <summary>
    /// Provides abstraction for the implementation of a process flow
    /// </summary>
    internal class ThoughtProcess {

        protected int ProcessStep;
        protected List<Action> ProcessList;

        /// <summary>
        /// Creates a new thought process
        /// </summary>
        public ThoughtProcess() {
            ProcessStep = 0;
            ProcessList = new List<Action>();
        }

        /// <summary>
        /// Moves to the next action in the process flow
        /// </summary>
        public void NextStep() {
            ProcessStep++;
        }

        /// <summary>
        /// Runs the current action in the process flow
        /// </summary>
        public void RunCurrentProcess() {
            if (ProcessStep < ProcessList.Count) {
                //perform current step in process flow
                callProcess(ProcessStep);
            } else {
                //reached end of process flow
                //Boid should now have satisfied need
                //in which case mind will discard this process
                //if mind decides the need is not satisfied:
                //  Reset to zero, so we can start again
                ProcessStep = 0;
            }
        }

        /// <summary>
        /// Runs the action at position <paramref name="step"/> in the process flow
        /// </summary>
        /// <param name="step">the index of the action to run</param>
        private void callProcess(int step) {
            (ProcessList[step])();
        }
    }
}
