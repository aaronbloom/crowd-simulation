using System.Collections.Generic;
using UnityEngine;

public class Graph {

    public List<Node> Nodes { get; private set; }

    public Graph(List<Node> nodes) {
        this.Nodes = nodes;
    }

    public static Graph ConstructGraph(Environment environment) {
        Vector2 dimensions = environment.GetFloorDimentions();
        int graphWidthInNodes = (int) dimensions.x;
        int graphHeightInNodes = (int) dimensions.y;

        List<Node> nodes = generateLatticeGraph(graphWidthInNodes, graphHeightInNodes);

        return new Graph(nodes);
    }

    private static List<Node> generateNodeGraph(int width, int height) {
        List<Node> nodes = new List<Node>();
        for (int a = 0; a < width; a++) {
            for (int b = 0; b < height; b++) {
                nodes.Add(new Node(new Vector3(a, 0, b)));
            }
        }
        return nodes;
    }

    private static List<Node> generateLatticeGraph(int width, int height) {
        List<Node> nodes = generateNodeGraph(width, height);

        //only need to look right and down per node to constuct full lattice
        for (int a = 0; a < width; a++) {
            for (int b = 0; b < height; b++) {
                if (a + 1 < width) { //node to the right
                    nodes[a * b].addTransition(nodes[(a + 1) * b]);
                }
                if (a + 1 < height) { //node underneath
                    nodes[a * b].addTransition(nodes[(a + 1) * b]);
                }
            }
        }
        return nodes;
    }

}
