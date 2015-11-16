using System.Collections.Generic;
using UnityEngine;

public class Graph {

    public List<Node> Nodes { get; private set; }

    public Graph(List<Node> nodes) {
        this.Nodes = nodes;
    }

    public static Graph constructGraph(Environment environment) {
        //work out width and height of 
        int graphWidthInNodes = (int) (environment.Bounds.x - environment.Origin.x);
        int graphHeightInNodes = (int) (environment.Bounds.z - environment.Origin.z);

        //loop through and generate nodes
        List<Node> nodes = new List<Node>();
        for (int a = 0; a < graphWidthInNodes; a++) {
            for (int b = 0; b < graphHeightInNodes; b++) {
                nodes.Add(new Node(new Vector3(a, 0, b)));
            }
        }

        //loop through and add transitions between nodes
        for (int a = 0; a < graphWidthInNodes; a++) {
            for (int b = 0; b < graphHeightInNodes; b++) {
                if(a + 1 < graphHeightInNodes) { //node to the right
                    nodes[a * b].addTransition(nodes[(a + 1) * b]);
                }
                if (a + 1 < graphHeightInNodes) { //node underneath
                    nodes[a * b].addTransition(nodes[(a + 1) * b]);
                }
            }
        }

        return new Graph(nodes);
    }

}
