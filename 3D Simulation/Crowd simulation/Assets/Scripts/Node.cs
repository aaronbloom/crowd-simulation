using UnityEngine;
using System.Collections.Generic;

public class Node {

    public Vector3 Position { get; private set; }
    public List<Transition> Transitions { get; private set; }

    public Node (Vector3 position) {
        this.Position = position;
    }

    public Node(Vector3 position, List<Transition> transitions) {
        this.Position = position;
        this.Transitions = transitions;
    }

    public void addTransition(Node target) {
        if (hasTransition(target)) {
            var transition = new Transition(this, target);
            target.Transitions.Add(transition);
            Transitions.Add(transition);
        }
    }

    private bool hasTransition(Node target) {
        foreach (var transition in Transitions) {
            if (transition.Nodes[0] == target || transition.Nodes[1] == target) return true;
        }
        return false;
    }
}
