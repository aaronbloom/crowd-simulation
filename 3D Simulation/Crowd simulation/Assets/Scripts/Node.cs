using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
        this.Transitions = new List<Transition>();
    }

    public Node(Vector3 position, Dictionary<Node, Transition> transitions) {
        this.Position = position;
        this.Transitions = transitions;
    }

    public void addTransition(Node target) {
        if (!hasTransition(target)) {
            var transition = new Transition(this, target);
            target.Transitions.Add(this, transition);
            Transitions.Add(target, transition);
        }
    }

    private bool hasTransition(Node target) {
        foreach (var transition in Transitions.Values) {
            if (transition.Nodes[0] == target || transition.Nodes[1] == target) return true;
        }
        return false;
    }
}
