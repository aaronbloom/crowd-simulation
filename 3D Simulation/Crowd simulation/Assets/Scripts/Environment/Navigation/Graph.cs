using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Environment.World.Objects;
using UnityEngine;

namespace Assets.Scripts.Environment.Navigation {

    /// <summary>
    /// A class to represent a graph of nodes
    /// </summary>
    public class Graph {

        /// <summary>
        /// List of Nodes that constitute the graph
        /// </summary>
        public List<Node> Nodes { get; private set; }

        //Creates new graph from a node list
        public Graph(List<Node> nodes) {
            this.Nodes = nodes;
        }

        /// <summary>
        /// Constructs a graph to fit an environment (<paramref name="environment"/>) with fidelity given by <paramref name="nodesPerMeter"/>
        /// </summary>
        /// <param name="environment">The environment to fill</param>
        /// <param name="nodesPerMeter">The fidelity of the graph</param>
        /// <returns>The constructed graph</returns>
        public static Graph ConstructGraph(Environment environment, float nodesPerMeter) {
            Vector2 dimensions = environment.FloorDimentions;
            int graphWidthInNodes = (int) (dimensions.x * nodesPerMeter);
            int graphHeightInNodes = (int) (dimensions.y * nodesPerMeter);

            List<Node> nodes = generateLatticeGraph(graphWidthInNodes, graphHeightInNodes, nodesPerMeter);

            return new Graph(nodes);
        }

        /// <summary>
        /// Returns the node in the graph closest to <paramref name="position"/>
        /// </summary>
        /// <param name="position">The Vector3 position</param>
        /// <returns>The closest node</returns>
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

        /// <summary>
        /// Removes Nodes in the graph that are within the bounds of <paramref name="collidable"/>
        /// </summary>
        /// <param name="collidable">the object within which to remove nodes</param>
        public void Cull(ICollidable collidable) {
            WorldObject obj = collidable.GetObject();
            Vector3 position = ZeroY(obj.GameObject.transform.position);
            Vector3 size = ZeroY(obj.Size);
            List<Node> culled = getCulledNodes(position, size);
            removeCulledNodes(culled);
        }

        /// <summary>
        /// <para>Generates a lattice of Nodes with the <paramref name="width"/> and <paramref name="height"/> at a fidelity of <paramref name="nodesPerMeter"/></para>
        /// <para>These nodes each have transitions to the nodes directly surrounding them</para>
        /// </summary>
        /// <param name="width">The width of the lattice</param>
        /// <param name="height">The height of the lattice</param>
        /// <param name="nodesPerMeter">the amount of nodes per metre</param>
        /// <returns>The finished List of nodes</returns>
        private static List<Node> generateLatticeGraph(int width, int height, float nodesPerMeter) {
            List<Node> nodes = generateNodeGraph(width, height, nodesPerMeter);
            for (int nodeX = 0; nodeX < width; nodeX++) {
                for (int nodeY = 0; nodeY < height; nodeY++) {
                    addNodeTransitions(width, height, nodes, nodeX, nodeY);
                }
            }
            return nodes;
        }

        /// <summary>
        /// Generates a grid of Nodes with the <paramref name="width"/> and <paramref name="height"/> at a fidelity of <paramref name="nodesPerMeter"/>
        /// </summary>
        /// <param name="width">The width of the grid</param>
        /// <param name="height">The height of the grid</param>
        /// <param name="nodesPerMeter">the amount of nodes per metre</param>
        /// <returns>The finished List of nodes</returns>
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

        /// <summary>
        /// Adds transitions to a node [<paramref name="nodeX"/>,<paramref name="nodeY"/> in the list <paramref name="nodes"/> to all surrounding nodes
        /// </summary>
        /// <param name="width">grid width</param>
        /// <param name="height">grid height</param>
        /// <param name="nodes">list of nodes</param>
        /// <param name="nodeX">Node x index in the grid</param>
        /// <param name="nodeY">Node y index in the grid</param>
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
            if ((nodeX + 1 < width) && (nodeY + 1 < height)) {
                Node bottomRightNeighbour = nodes[((nodeX + 1) * width) + nodeY + 1];
                thisNode.AddTransition(bottomRightNeighbour);
            }
            if ((nodeX - 1 > 0) && (nodeY + 1 < height)) {
                Node bottomLeftNeighbour = nodes[((nodeX - 1) * width) + nodeY + 1];
                thisNode.AddTransition(bottomLeftNeighbour);
            }
        }

        /// <summary>
        /// Renders the graph transitions to the screen for debugging purposes
        /// </summary>
        public void DrawGraphGizmo() {
            foreach (KeyValuePair<Node, Transition> entry in Nodes.SelectMany(node => node.Transitions)) {
                Gizmos.DrawLine(entry.Value.Nodes[0].Position, entry.Value.Nodes[1].Position);
            }
        }

        /// <summary>
        /// turns a vector (x,y,z) to (x,0,z).
        /// </summary>
        /// <param name="vector">the vector to y->0</param>
        /// <returns><paramref name="vector"/> with a 0 y value</returns>
        private static Vector3 ZeroY(Vector3 vector) {
            return new Vector3(vector.x, 0, vector.z);
        }

        /// <summary>
        /// Returns a list of nodes that will be culled with a certain distance (<paramref name="size"/>) of a position (<paramref name="position"/>)
        /// </summary>
        /// <param name="position">the root location of the culling</param>
        /// <param name="size">the radius of the culling</param>
        /// <returns>the nodes which will be culled</returns>
        private List<Node> getCulledNodes(Vector3 position, Vector3 size) {
            return Nodes.Where(node => (position - node.Position).magnitude <= (size/2).magnitude).ToList();
        }

        /// <summary>
        /// Removes the nodes given in <paramref name="culled"/> from the graph
        /// </summary>
        /// <param name="culled"></param>
        private void removeCulledNodes(IEnumerable<Node> culled) {
            foreach (Node node in culled) {
                node.Disconnect();
                Nodes.Remove(node);
            }
        }
    }
}
