﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Boid.ThoughtProcesses
{
    class ToiletProcess : ThoughtProcess
    {
        private Boid owner;
        private Need ownerDesire;

        public ToiletProcess(Boid boid, Need toSatisfy) : base()
        {
            owner = boid;
            processList.Add((Action) navigateToToilet);
            processList.Add((Action) continueWalkToToilet);
            processList.Add((Action) reachToilet);
        }

        private void navigateToToilet()
        {
            GoalSeekingBehaviour gsb = new GoalSeekingBehaviour(owner, 10f, 2.5f); //fekin magic numbers, thanks Aaron
            gsb.ChooseClosestFromList(BootStrapper.EnvironmentManager.CurrentEnvironment.World.Toilets);
            owner.behaviour = gsb;
            NextStep();
        }

        private void continueWalkToToilet()
        {
            if (owner.behaviour.BehaviourComplete)
            {
                NextStep();
            }
        }

        private void reachToilet()
        {
            ownerDesire.Satisfy();
            NextStep();
        }

    }
}
