using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Environment.Navigation {

    /// <summary>
    /// A class to represent a point in a graph, with tranistions to other points
    /// </summary>
    public class Node {

        public Vector3 Position { get; private set; }
        public Dictionary<Node, Transition> Transitions { get; private set; }
        public List<Node> TransitionsTo { get {return Transitions.Keys.ToList();}}

        /// <summary>
        /// Creates a new Node at position: <paramref name="position"/>
        /// </summary>
        /// <param name="position">The Vector3 position where the node shall be</param>
        public Node (Vector3 position) {
            this.Position = position;
            this.Transitions = new Dictionary<Node, Transition>();
        }

        /// <summary>
        /// Creates a new node as position: <paramref name="position"/> with the transitions given in: <paramref name="transitions"/>
        /// </summary>
        /// <param name="position"></param>
        /// <param name="transitions"></param>
        public Node(Vector3 position, Dictionary<Node, Transition> transitions) {
            this.Position = position;
            this.Transitions = transitions;
        }

        /// <summary>
        /// Adds a transition from this node to the <paramref name="target"/>
        /// </summary>
        /// <param name="target">The node to connect to</param>
        public void AddTransition(Node target) {
            if (!hasTransition(target)) {
                var transition = new Transition(this, target);
                target.Transitions.Add(this, transition);
                Transitions.Add(target, transition);
            }
        }

        /// <summary>
        /// Finds the distance between 2 nodes
        /// </summary>
        /// <param name="a">First Node</param>
        /// <param name="b">Second Node</param>
        /// <returns>Distance between node params</returns>
        public static float distanceBetween(Node a, Node b) {
            return (a.Position - b.Position).magnitude;
        }

        /// <summary>
        /// Deletes all transitions to and from this node
        /// </summary>
        internal void Disconnect() {
            //Avoid out of sync error by storing in list
            List<Transition> transitions = Transitions.Values.ToList();
            foreach (Transition t in transitions) {
                removeTransition(t);
            }
        }

        /// <summary>
        /// Removes a single transition (<paramref name="transition"/> to and from this node
        /// </summary>
        /// <param name="transition">The transition to remove</param>
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

        /// <summary>
        /// sees if the node has a connection a node <paramref name="target"/>
        /// </summary>
        /// <param name="target"></param>
        /// <returns>true if the node is connected to <paramref name="target"/></returns>
        private bool hasTransition(Node target) {
            return Transitions.Values.Any(transition => (transition.Nodes[0] == target) || (transition.Nodes[1] == target));
        }
    }
}
