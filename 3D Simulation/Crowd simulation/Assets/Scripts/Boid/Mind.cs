using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Boid.ThoughtProcesses;
using Assets.Scripts.Environment.World.Objects;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {
    public class Mind {

        public Goal Goal { get; private set; }
        public Need CurrentNeed { get; private set; }
        public float Thirst { get { return drinkNeed.Value; } }
        public float ToiletNeed { get { return toiletNeed.Value; } }
        public float DanceNeed { get { return danceNeed.Value; } }

        private Boid boid;
        private ThoughtProcess thoughtProcess;

        private Need drinkNeed;
        private Need toiletNeed;
        private Need danceNeed;

        public Mind(Boid boid) {
            this.boid = boid;
            drinkNeed = new Need(MindState.Thirsty, Random.Range(0.1f, boid.Properties.DrinkNeedRate), 100);
            toiletNeed = new Need(MindState.Incontenent, Random.Range(0.1f, boid.Properties.ToiletNeedRate), 100);
            danceNeed = new Need(MindState.Dancey, Random.Range(0.1f, boid.Properties.DanceNeedRate), 100);

            CurrentNeed = danceNeed;
            startNewProcess(CurrentNeed.MindState);
        }

        private MindState evaluatePriorities() {
            if (CurrentNeed.Satisfied)
            {
                if (drinkNeed.Value > CurrentNeed.Value) CurrentNeed = drinkNeed;
                if (toiletNeed.Value > CurrentNeed.Value) CurrentNeed = toiletNeed;
                if (danceNeed.Value > CurrentNeed.Value) CurrentNeed = danceNeed;
            }
            return CurrentNeed.MindState;

        }

        private void desireNeeds() {
            drinkNeed.Desire();
            toiletNeed.Desire();
            danceNeed.Desire();
        }

        public void Think() {
            desireNeeds();
            MindState oldMindState = CurrentNeed.MindState;
            MindState newMindState = evaluatePriorities();
            if (newMindState != oldMindState) {
                //Needs have changed - update behaviours
                startNewProcess(newMindState);
            }
            thoughtProcess.RunCurrentProcess();
        }

        private void startNewProcess(MindState mindState) {
            switch (mindState) {
                case MindState.Thirsty:
                    thoughtProcess = new BarProcess(boid, drinkNeed);
                    break;
                case MindState.Dancey:
                    thoughtProcess = new DanceProcess(boid, danceNeed);
                    break;
                case MindState.Incontenent:
                    thoughtProcess = new ToiletProcess(boid, toiletNeed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }

    public class Need {

        public MindState MindState { get; private set; }
        public float Value { get; private set; }
        public float Max { get; private set; }
        public float Min { get; private set; }

        public bool Satisfied {
            get { return Value < satisfactionThreshold; }
        }

        private readonly float increment;
        private readonly float satisfactionThreshold;

        public Need(MindState mindState, float increment, float satisfactionThreshold) {
            this.Max = 5000000;
            this.Min = 0;
            this.increment = increment;
            this.satisfactionThreshold = satisfactionThreshold;
            this.MindState = mindState;
        }

        //Increases Need
        public void Desire() {
            if (Value < Max) Value += increment;
        }

        //Decreases Need to minimum value
        public void SatisfyCompletely() {
            Value = Min;
        }

        //Decreases Need to point of satisfaction
        public void SatifyToThreshhold() {
            Value = Math.Max(Min,satisfactionThreshold-20);
        }
        
        //Decreases Need by value
        public void SatisfyByValue(int val) {
            Value-= val;
            if (Value < 0) Value = 0;
        }

        //Decreases Need by percent of current value
        public void SatisfyByPercent(float perc)
        {
            Value = (int) (Value * perc);
        }
    }

    public enum MindState {
        Thirsty,
        Dancey,
        Incontenent
    }
}
