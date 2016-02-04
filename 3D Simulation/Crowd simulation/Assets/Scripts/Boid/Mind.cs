﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Boid.ThoughtProcesses;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid {

    class Mind {

        public Goal Goal { get; private set; }
        public Need CurrentNeed { get; private set; }

        private Boid boid;
        private ThoughtProcess thoughtProcess;

        private Need drinkNeed;
        private Need toiletNeed;
        private Need danceNeed;

        public Mind(Boid boid) {
            this.boid = boid;
            drinkNeed = new Need(MindState.Thirsty, 1, 20);
            toiletNeed = new Need(MindState.Incontenent, 1, 20);
            danceNeed = new Need(MindState.Dancey, 2, 20);

            CurrentNeed = danceNeed;
            startNewProcess(CurrentNeed.MindState);
        }

        private MindState evaluatePriorities() {
            //if (CurrentNeed == null) CurrentNeed = danceNeed;
            if (drinkNeed.Value > CurrentNeed.Value) CurrentNeed = drinkNeed;
            if (toiletNeed.Value > CurrentNeed.Value) CurrentNeed = toiletNeed;
            if (danceNeed.Value > CurrentNeed.Value) CurrentNeed = danceNeed;
            //if (CurrentNeed.Satisfied) CurrentNeed = null;
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

    class Need {
        public MindState MindState { get; private set; }
        public int Value { get; private set; }
        public int Max { get; private set; }
        public int Min { get; private set; }

        public bool Satisfied {
            get { return Value < satisfactionThreshold; }
        }

        private readonly int increment;
        private readonly int satisfactionThreshold;


        public Need(MindState mindState, int increment, int satisfactionThreshold) {
            this.Max = 5000000;
            this.Min = 0;
            this.increment = increment;
            this.satisfactionThreshold = satisfactionThreshold;
            this.MindState = mindState;
        }

        public void Desire() {
            if (Value < Max) Value += increment;
        }

        public void Satisfy() {
            Value = Min;
        }
    }

    enum MindState {
        Thirsty,
        Dancey,
        Incontenent
    }
}
