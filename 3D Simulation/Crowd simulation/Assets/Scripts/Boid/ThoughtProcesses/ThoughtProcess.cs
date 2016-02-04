using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Boid
{
    class ThoughtProcess
    {
        protected int processStep;
        protected List<object> processList;

        public ThoughtProcess()
        {
            processStep = 0;
            processList = new List<object>();
        }

        public void NextStep()
        {
            processStep++;
        }

        public void RunCurrentProcess()
        {
            if (processStep < processList.Count)
            {
                //perform current step in process flow
                callProcess(processStep);
            }
            else
            {
                //reached end of process flow
                //boid should now have satisfied need
                //in which case mind will discard this process
                //if mind decides the need is not satisfied:
                //  Reset to zero, so we can start again
                processStep = 0;
            }
        }

        private void callProcess(int step)
        {
            ((Action) processList[step])();
        }
    }
}
