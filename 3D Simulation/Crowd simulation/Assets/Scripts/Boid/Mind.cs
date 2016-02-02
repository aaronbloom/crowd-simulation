using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Environment.World.Objects;

namespace Assets.Scripts.Boid
{
    class Mind
    {
        public Goal Goal { get; private set; }
        public Need currentNeed { get; private set; }
        private Boid boid;

        private Need drinkNeed;
        private Need toiletNeed;
        private Need danceNeed;

        public Mind(Boid boid)
        {
            this.boid = boid;
            drinkNeed = new Need(MindState.Thirsty, 1);
            toiletNeed = new Need(MindState.Incontenent, 1);
            danceNeed = new Need(MindState.Dancey, 2);
            evaluatePriorities();
        }

        private void evaluatePriorities()
        {
            if (drinkNeed.Value > currentNeed.Value) currentNeed = drinkNeed;
            if (toiletNeed.Value > currentNeed.Value) currentNeed = toiletNeed;
            if (danceNeed.Value > currentNeed.Value) currentNeed = danceNeed;
        }

        private void desireNeeds()
        {
            drinkNeed.Desire();
            toiletNeed.Desire();
            danceNeed.Desire();
        }

    }

    class Need
    {
        public int Value { get; private set; }
        public int Max { get; private set; }
        public int Min { get; private set; }
        private readonly int increment;
        public MindState StateTag { get; private set; }

        public Need(MindState stateTag, int increment)
        {
            this.Max = 500;
            this.Min = 0;
            this.increment = increment;
            this.StateTag = stateTag;
        }

        public void Desire()
        {
            if (Value < Max) Value += increment;
        }

        public void Satisfy()
        {
            Value = Min;
        }
    }

    enum MindState
    {
        Thirsty, Dancey, Incontenent
    }
}
