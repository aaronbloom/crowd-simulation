using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Environment.Navigation {
    public class Node {

        public Vector3 Position { get; private set; }
        public Dictionary<Node, Transition> Transitions { get; private set; }
        public List<Node> TransitionsTo {
            get {
                return Transitions.Keys.ToList();
            }
        }

        public Node (Vector3 position) {
            this.Position = position;
            this.Transitions = new Dictionary<Node, Transition>();
        }

        public Node(Vector3 position, Dictionary<Node, Transition> transitions) {
            this.Position = position;
            this.Transitions = transitions;
        }

        public void AddTransition(Node target) {
            if (!hasTransition(target)) {
                var transition = new Transition(this, target);
                target.Transitions.Add(this, transition);
                Transitions.Add(target, transition);
            }
        }

        private void removeTransition(Transition transition) {
            Node otherNode;
            if (transition.Nodes[0] == this) {
                otherNode = transition.Nodes[1];
            } else {
                otherNode = transition.Nodes[0];
            }
            otherNode.Transitions.Remove(this);
            this.Transitions.Remove(otherNode);
        }

        private bool hasTransition(Node target) {
            foreach (var transition in Transitions.Values) {
                if (transition.Nodes[0] == target || transition.Nodes[1] == target) return true;
            }
            return false;
        }

        internal void Disconnect() {
            //Avoid out of sync error by storing in list
            List<Transition> transitions = Transitions.Values.ToList();
            foreach(Transition t in transitions) {
                removeTransition(t);
            }
        }

        public static float distanceBetween(Node a, Node b) {
            return (a.Position - b.Position).magnitude;
        }
    }
}
