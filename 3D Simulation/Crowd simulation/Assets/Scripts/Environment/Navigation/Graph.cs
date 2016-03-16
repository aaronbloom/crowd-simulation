using System.Collections.Generic;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.Navigation {
    public class Graph {

        public List<Node> Nodes { get; private set; }

        public Graph(List<Node> nodes) {
            this.Nodes = nodes;
        }

        public static Graph ConstructGraph(Environment environment, float nodesPerMeter) {
            Vector2 dimensions = environment.FloorDimentions;
            int graphWidthInNodes = (int) (dimensions.x * nodesPerMeter);
            int graphHeightInNodes = (int) (dimensions.y * nodesPerMeter);

            List<Node> nodes = generateLatticeGraph(graphWidthInNodes, graphHeightInNodes, nodesPerMeter);

            return new Graph(nodes);
        }

        public Node FindClosestNode(Vector3 position) {
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

        public void Cull(Collidable collidable) {
            WorldObject obj = collidable.getObject();
            Vector3 position = ZeroY(obj.GameObject.transform.position);
            Vector3 size = ZeroY(obj.Size);
            List<Node> culled = getCulledNodes(position, size);
            removeCulledNodes(culled);
        }

        private Vector3 ZeroY(Vector3 vector) {
            return new Vector3(vector.x, 0, vector.z);
        }

        private List<Node> getCulledNodes(Vector3 position, Vector3 size) {
            List<Node> culled = new List<Node>();
            foreach (Node node in Nodes) {
                if ((position - node.Position).magnitude <= (size / 2).magnitude) {
                    culled.Add(node);
                }
            }

            return culled;
        }

        private void removeCulledNodes(List<Node> culled) {
            foreach (Node node in culled) {
                node.Disconnect();
                Nodes.Remove(node);
            }
        }

        private static List<Node> generateLatticeGraph(int width, int height, float nodesPerMeter) {
            List<Node> nodes = generateNodeGraph(width, height, nodesPerMeter);
            for (int nodeX = 0; nodeX < width; nodeX++) {
                for (int nodeY = 0; nodeY < height; nodeY++) {
                    addNodeTransitions(width, height, nodes, nodeX, nodeY);
                }
            }
            return nodes;
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

        private static void addNodeTransitions(int width, int height, List<Node> nodes, int nodeX, int nodeY) {
            Node thisNode = nodes[(nodeX * width) + nodeY];
            if (nodeX + 1 < width) {
                Node rightNeighbour = nodes[((nodeX + 1) * width) + nodeY];
                thisNode.AddTransition(rightNeighbour);
            }
            if (nodeY + 1 < height) {
                Node bottomNeighbour = nodes[(nodeX * width) + nodeY + 1];
                thisNode.AddTransition(bottomNeighbour);
            }
            if (nodeX + 1 < width && nodeY + 1 < height) {
                Node bottomRightNeighbour = nodes[((nodeX + 1) * width) + nodeY + 1];
                thisNode.AddTransition(bottomRightNeighbour);
            }
            if (nodeX - 1 > 0 && nodeY + 1 < height) {
                Node bottomLeftNeighbour = nodes[((nodeX - 1) * width) + nodeY + 1];
                thisNode.AddTransition(bottomLeftNeighbour);
            }
        }

        public void DrawGraphGizmo() {
            foreach (Node node in Nodes) {
                foreach (KeyValuePair<Node, Transition> entry in node.Transitions) {
                    //Gizmos.DrawLine(entry.Value.Nodes[0].Position, entry.Value.Nodes[1].Position);
                }
            }
        }

    }
}
