using System;
using Assets.Scripts.Boid.ThoughtProcesses;
using Assets.Scripts.Environment.World.Objects;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Boid {

    /// <summary>
    /// This class gives a boid the ability to make reasoned decisions on what to do
    /// </summary>
    public class Mind {

        public Goal Goal { get; private set; }
        public Need CurrentNeed { get; private set; }
        public float Thirst { get { return drinkNeed.Value; } }
        public float ToiletNeed { get { return toiletNeed.Value; } }
        public float DanceNeed { get { return danceNeed.Value; } }

        private readonly Boid boid;
        private ThoughtProcess thoughtProcess;

        private readonly Need drinkNeed;
        private readonly Need toiletNeed;
        private readonly Need danceNeed;

        /// <summary>
        /// Creates new mind object for <paramref name="boid"/>
        /// </summary>
        /// <param name="boid"></param>
        public Mind(Boid boid) {
            this.boid = boid;

            drinkNeed = new Need(MindState.Thirsty, Random.Range(0.1f, boid.Properties.DrinkNeedRate));
            toiletNeed = new Need(MindState.Incontenent, Random.Range(0.1f, boid.Properties.ToiletNeedRate));
            danceNeed = new Need(MindState.Dancey, Random.Range(0.1f, boid.Properties.DanceNeedRate));

            desireNeeds();
            CurrentNeed = danceNeed;
            startNewProcess(evaluatePriorities());
        }

        /// <summary>
        /// This will cause the mind to cycle execution once, running one process flow step
        /// </summary>
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

        /// <summary>
        /// This will see if the current need is not the strongest desired, and change it if so
        /// </summary>
        /// <returns>updated current need</returns>
        private MindState evaluatePriorities() {
            if (!CurrentNeed.Satisfied) return CurrentNeed.MindState;
            //else
            if (drinkNeed.Value > CurrentNeed.Value) CurrentNeed = drinkNeed;
            if (toiletNeed.Value > CurrentNeed.Value) CurrentNeed = toiletNeed;
            if (danceNeed.Value > CurrentNeed.Value) CurrentNeed = danceNeed;
            return CurrentNeed.MindState;

        }

        /// <summary>
        /// increases the desire for all needs
        /// </summary>
        private void desireNeeds() {
            drinkNeed.Desire();
            toiletNeed.Desire();
            danceNeed.Desire();
        }

        /// <summary>
        /// begins a new process flow, usually on the event of a current need change
        /// </summary>
        /// <param name="mindState">The new current need type</param>
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

    /// <summary>
    /// This class represents a boid's need, and it's desire for it.
    /// </summary>
    public class Need {

        public static int DefaultThreshold = 100;
        public static int ThresholdModifier = 20;

        public MindState MindState { get; private set; }
        public float Value { get; private set; }
        public float Max { get; private set; }
        public float Min { get; private set; }
        
        /// <returns>if this need meets it's satisfaction criteria</returns>
        public bool Satisfied {
            get { return Value < satisfactionThreshold; }
        }

        private readonly float increment;
        private readonly float satisfactionThreshold;

        /// <summary>
        /// Create new need with:
        /// </summary>
        /// <param name="mindState">Need type</param>
        /// <param name="increment">Need desire rate</param>
        public Need(MindState mindState, float increment) {
            this.Max = 5000000;
            this.Min = 0;
            this.increment = increment;
            this.satisfactionThreshold = DefaultThreshold;
            this.MindState = mindState;
        }

        /// <summary>
        /// Increases Need desire
        /// </summary>
        public void Desire() {
            if (Value < Max) Value += increment;
        }

        /// <summary>
        /// Decreases Need to minimum value
        /// </summary>
        public void SatisfyCompletely() {
            Value = Min;
        }

        /// <summary>
        /// Decreases Need to point of satisfaction
        /// </summary>
        public void SatifyToThreshhold() {
            Value = Math.Max(Min,satisfactionThreshold-ThresholdModifier);
        }

        /// <summary>
        /// Decreases Need by <paramref name="val"/>
        /// </summary>
        /// <param name="val">absolute value to decrease need by</param>
        public void SatisfyByValue(int val) {
            Value-= val;
            if (Value < 0) Value = 0;
        }

        /// <summary>
        /// Decreases Need by <paramref name="perc"/>%
        /// </summary>
        /// <param name="perc">percentage to decrease need by</param>
        public void SatisfyByPercent(float perc)
        {
            Value = (int) (Value * perc);
        }
    }

    /// <summary>
    /// The different need types
    /// </summary>
    public enum MindState {
        Thirsty,
        Dancey,
        Incontenent
    }
}
