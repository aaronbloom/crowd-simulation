using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Boid
{
    class ThoughtProcess
    {
        private int processStep;
        private List<object> processList;

        public void NextStep()
        {
            processStep++;
        }

        public void RunCurrentProcess()
        {
            callProcess(processStep);
        }

        private void callProcess(int step)
        {
            ((Action) processList[step])();
        }
    }
}
