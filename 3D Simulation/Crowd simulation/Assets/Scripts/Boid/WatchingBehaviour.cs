using System;
using Assets.Scripts.Environment.Navigation;
using UnityEngine;

namespace Assets.Scripts.Boid {
    internal class WatchingBehaviour : BoidBehaviour {

        private readonly Node goalNode;

        public WatchingBehaviour(Boid boid, Node goalNode) {
            this.boid = boid;
            this.goalNode = goalNode;
        }

        public override Vector3 InitialVelocity() {
            return Vector3.zero;
        }

        public override Vector3 updateAcceleration() {
            boid.transform.LookAt(goalNode.Position);
            return Vector3.zero;
        }

        public override void DrawGraphGizmo() {}
    }
}
