using System;
using System.Collections.Generic;

namespace Assets.Scripts.Boid.ThoughtProcesses {
    internal class ThoughtProcess {

        protected int ProcessStep;
        protected List<object> ProcessList;

        public ThoughtProcess() {
            ProcessStep = 0;
            ProcessList = new List<object>();
        }

        public void NextStep() {
            ProcessStep++;
        }

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

        private void callProcess(int step) {
            ((Action)ProcessList[step])();
        }
    }
}
