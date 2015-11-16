using System.Collections.Generic;
using UnityEngine;

public class Graph {

    public List<Node> Nodes { get; private set; }

    public Graph(List<Node> nodes) {
        this.Nodes = nodes;
    }

}
