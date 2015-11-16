using UnityEngine;
using System.Collections;

public class Transition {

    public Node[] Nodes { get; private set; }

    public Transition (Node one, Node two) {
        Nodes = new Node[2] {one, two};
    }
}
