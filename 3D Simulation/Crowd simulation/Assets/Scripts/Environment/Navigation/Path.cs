using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.Environment.Navigation {

    /// <summary>
    /// A class to represent a linear walk through a graph
    /// </summary>
    internal class Path : Graph {

        private const float NavigationAccuracy = 0.5f;

        /// <summary>
        /// Constructs Path from ordered node list
        /// </summary>
        /// <param name="nodes">The nodes to be in the path</param>
        public Path(List<Node> nodes) : base(nodes) {}

        /// <summary>
        /// Uses A* to navigate with a Graph (<paramref name="graph"/>), starting from <paramref name="startNode"/> ending at <paramref name="goalNode"/>
        /// </summary>
        /// <param name="graph">The Navigational Graph</param>
        /// <param name="startNode">The Node to start at</param>
        /// <param name="goalNode">The Node to end at</param>
        /// <returns>The Subgraph of the <paramref name="graph"/> which maps a route from <paramref name="startNode"/> to <paramref name="goalNode"/></returns>
        public static Path Navigate(Graph graph, Node startNode, Node goalNode) {

            Dictionary<Vector3, HeuristicalNode> closedSet = new Dictionary<Vector3, HeuristicalNode>();
            Dictionary<Vector3, HeuristicalNode> openSet = new Dictionary<Vector3, HeuristicalNode>();

            HeuristicalNode start = new HeuristicalNode(startNode);
            HeuristicalNode goal = new HeuristicalNode(goalNode);

            //Special case for startNode
            openSet.Add(start.Node.Position, start);
            start.ValueF = 0;
            start.Parent = null;

            //Process Open Set
            while (openSet.Values.Count > 0) {

                //Choose most promising node
                HeuristicalNode mostPromising = lowestFScoreInSet(openSet.Values.ToList()); //needs renaming

                //Remove most promising node from open set
                openSet.Remove(mostPromising.Node.Position);

                //Process Nodes which most Promising Node connects to
                foreach (Node candidatePromisingNode in mostPromising.Node.TransitionsTo) {
                    HeuristicalNode candidatePromising = new HeuristicalNode(candidatePromisingNode);

                    //See if we're at our destination
                    if (candidatePromising.Node.Position == goal.Node.Position) {
                        //return the path to the goal from start
                        candidatePromising.Parent = mostPromising;
                        return new Path(convertParentageToList(candidatePromising));
                    }

                    //calculate heuristics for candiate node
                    candidatePromising.SetG(mostPromising);
                    candidatePromising.SetH(goal);
                    float potentialF = candidatePromising.CalculateF();

                    //check this is the best route we know to the candidate
                    if (openSet.ContainsKey(candidatePromising.Node.Position) && openSet[candidatePromising.Node.Position].ValueF < potentialF && Random.Range(0f,1f) > NavigationAccuracy) {
                        //Not interested
                    } else if (closedSet.ContainsKey(candidatePromising.Node.Position) && closedSet[candidatePromising.Node.Position].ValueF < potentialF) {
                        //Not interested
                    } else {
                        //this is the best route to the candidate node
                        candidatePromising.SetF();

                        //set parent for pathing
                        candidatePromising.Parent = mostPromising;

                        //add to open set
                        if (openSet.ContainsKey(candidatePromising.Node.Position)) openSet.Remove(candidatePromising.Node.Position);
                        openSet.Add(candidatePromising.Node.Position, candidatePromising);

                    }
                }
                //all candidates processed, close node
                if (closedSet.ContainsKey(mostPromising.Node.Position)) closedSet.Remove(mostPromising.Node.Position);
                closedSet.Add(mostPromising.Node.Position, mostPromising);
            }
            throw new InvalidOperationException("Specified goalNode was not in the same navigational Graph as startNode");
        }

        /// <summary>
        /// Convert Linked List of HeuristicalNodes to Linear Ordered List of Nodes
        /// </summary>
        /// <param name="goal">The node at the end of the linked list</param>
        /// <returns>The entire list</returns>
        private static List<Node> convertParentageToList(HeuristicalNode goal) {
            List<Node> list = new List<Node> {goal.Node};
            for (HeuristicalNode n = goal; n != null; n = n.Parent) {
                list.Add(n.Node);
            }
            list.Reverse();
            return list;
        }

        /// <summary>
        /// Find the lowest scoring node in the provided set & dictionary.
        /// </summary>
        /// <param name="set">List of HeuristicalNodes to scan</param>
        /// <returns>The HeuristicalNode in the set with lowest f score</returns>
        private static HeuristicalNode lowestFScoreInSet(List<HeuristicalNode> set) {
            float lowest = float.MaxValue;
            HeuristicalNode winningNode = null;
            foreach (var node in set) {
                var value = node.ValueF;
                if (value <= lowest) {
                    lowest = value;
                    winningNode = node;
                }
            }
            return winningNode;
        }

        /// <summary>
        /// Extension for Node providing storage and calculation for pathing heuristics
        /// </summary>
        private class HeuristicalNode {

            public float ValueF { get; set; }
            public HeuristicalNode Parent { get; set; }
            public Node Node { get; private set; }

            private float ValueG { get; set; }
            private float ValueH { get; set; }

            /// <summary>
            /// Creates new HeuristicalNode
            /// </summary>
            /// <param name="node">The Node to wrap</param>
            public HeuristicalNode(Node node) {
                ValueG = 0;
                ValueH = 0;
                ValueF = float.MaxValue;
                Parent = null;
                Node = node;
            }

            /// <summary>
            /// calculates and sets g
            /// </summary>
            /// <param name="parentNode">The best node leading to this one</param>
            public void SetG(HeuristicalNode parentNode) {
                ValueG = calculateG(parentNode);
            }

            /// <summary>
            /// calcuates and sets h
            /// </summary>
            /// <param name="goalNode">The node we're trying to reach</param>
            public void SetH(HeuristicalNode goalNode) {
                ValueH = calculateH(goalNode);
            }

            /// <summary>
            /// calculates and sets f
            /// </summary>
            public void SetF() {
                ValueF = CalculateF();
            }

            /// <summary>
            /// calculates G (Cost Complete)
            /// </summary>
            /// <param name="parentNode">The best node leading to this one</param>
            /// <returns>The g value of this node</returns>
            private float calculateG(HeuristicalNode parentNode) {
                return parentNode.ValueG + distanceBetween(this.Node, parentNode.Node);
                //return distanceBetween(this.Node, startNode.Node);
            }

            /// <summary>
            /// calculates H (Cost remaining [guessed])
            /// </summary>
            /// <param name="goalNode">The node we're trying to reach</param>
            /// <returns>The h value of this node</returns>
            private float calculateH(HeuristicalNode goalNode) {
                return distanceBetween(this.Node, goalNode.Node);
            }

            /// <summary>
            /// calculates F (Total Cost)
            /// </summary>
            /// <returns>The f value of this node</returns>
            public float CalculateF() {
                return ValueG + ValueH;
            }

            /// <summary>
            /// Returns position Vector as string
            /// </summary>
            /// <returns>position vector string</returns>
            public override string ToString() {
                return Node.Position.ToString();
            }

            /// <summary>
            /// Return linear distance between 2 nodes
            /// </summary>
            /// <param name="a">Node A</param>
            /// <param name="b">Node B</param>
            /// <returns>distance between point A and B</returns>
            private static float distanceBetween(Node a, Node b) {
                return (a.Position - b.Position).magnitude;
            }

        }

    }
}

