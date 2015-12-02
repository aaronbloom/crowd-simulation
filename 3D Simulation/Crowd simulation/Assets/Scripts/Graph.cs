using System;
using System.Collections.Generic;
using Assets.Scripts.WorldObjects;
using UnityEngine;

public class Graph {

    public List<Node> Nodes { get; private set; }

    public Graph(List<Node> nodes) {
        this.Nodes = nodes;
    }

    public static Graph ConstructGraph(Environment environment, float nodesPerMeter) {
        Vector2 dimensions = environment.FloorDimentions;
        int graphWidthInNodes = (int) (dimensions.x * nodesPerMeter);
        int graphHeightInNodes = (int) (dimensions.y * nodesPerMeter);

        List<Node> nodes = generateRandomlyLinkedLatticeGraph(graphWidthInNodes, graphHeightInNodes, nodesPerMeter, 0.8f);

        return new Graph(nodes);
    }

    public Node FindClosestNode (Vector3 position) {
        Node closestFound = null;
        float closestDistance = float.MaxValue;
        foreach (Node node in Nodes) {
            float currentDistance = Vector3.Distance(position, node.Position);
            if (currentDistance < closestDistance) {
                closestDistance = currentDistance;
                closestFound = node;
            }
        }
        return closestFound;
    }

    public void Cull(Wall collidable) {
        Vector3 position = collidable.GameObject.transform.position;
        Vector3 size = collidable.GameObject.transform.localScale;
        Nodes.RemoveAll(node => (position - node.Position).magnitude < (size/2).magnitude);
    }

    private static List<Node> generateNodeGraph(int width, int height, float nodesPerMeter) {
        List<Node> nodes = new List<Node>();
        for (int a = 0; a < width; a++) {
            for (int b = 0; b < height; b++) {
                Vector3 position = new Vector3(a / nodesPerMeter, 0, b / nodesPerMeter);
                nodes.Add(new Node(position));
            }
        }
        return nodes;
    }

    private static List<Node> generateLatticeGraph(int width, int height, float nodesPerMeter) {
        List<Node> nodes = generateNodeGraph(width, height, nodesPerMeter);

        //only need to look right and down per node to constuct full lattice
        for (int a = 0; a < width; a++) {
            for (int b = 0; b < height; b++) {
                if (a + 1 < width) {
                    //node to the right
                    nodes[(a*width) + b].addTransition(nodes[((a + 1)*width) + b]);
                }
                if (b + 1 < height) {
                    //node underneath
                    nodes[(a*width) + b].addTransition(nodes[(a*width) + b + 1]);
                }
            }
        }
        return nodes;
    }

    private static List<Node> generateRandomlyLinkedLatticeGraph(int width, int height, float nodesPerMeter, float linkingProbability) {
        List<Node> nodes = generateNodeGraph(width, height, nodesPerMeter);

        //only need to look right and down per node to constuct full lattice
        for (int a = 0; a < width; a++) {
            for (int b = 0; b < height; b++) {
                if (UnityEngine.Random.Range(0f, 1f) < linkingProbability) {
                    if (a + 1 < width) {
                        //node to the right
                        nodes[(a * width) + b].addTransition(nodes[((a + 1) * width) + b]);
                    }
                }
                if (UnityEngine.Random.Range(0f, 1f) < linkingProbability) {
                    if (b + 1 < height) {
                        //node underneath
                        nodes[(a * width) + b].addTransition(nodes[(a * width) + b + 1]);
                    }
                }
            }
        }
        return nodes;
    }

    public void DrawGraphGizmo() {
        foreach (Node node in Nodes) {
            Gizmos.DrawSphere(node.Position, 1);
            foreach (KeyValuePair<Node, Transition> entry in node.Transitions) {
                Gizmos.DrawLine(entry.Value.Nodes[0].Position, entry.Value.Nodes[1].Position);
            }
        }
    }

}
