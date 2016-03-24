using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Environment.Navigation {
    class Path : Graph {

        private static readonly float navigationAccuracy = 0.5f;

        //Constructs Path from ordered node list
        public Path(List<Node> nodes) : base(nodes) {
        }

        public void DrawGraphGizmo() {
            foreach (Node node in Nodes) {
                foreach (KeyValuePair<Node, Transition> entry in node.Transitions) {
                    Gizmos.DrawLine(entry.Value.Nodes[0].Position, entry.Value.Nodes[1].Position);
                }
            }
            //Gizmos.DrawSphere(Nodes[Nodes.Count-1].Position, 1);
        }

        public static Path Loiter(Graph graph, Node loiterNode, int maxLoiterDist, int minLoiterNodes, int maxLoiterNodes) {

            List<Node> path = new List<Node>();

            Node currNode = loiterNode;
            path.Add(loiterNode);
            int nodesToLoiter = UnityEngine.Random.Range(minLoiterNodes, maxLoiterNodes);
            for (; nodesToLoiter > 0; nodesToLoiter--) {
                List<Node> potentialNodes = new List<Node>(currNode.TransitionsTo);
                while (potentialNodes.Count > 0) {
                    int index = (int)UnityEngine.Random.Range(0, potentialNodes.Count);
                    Node potentialNode = potentialNodes[index];
                    if (Node.distanceBetween(potentialNode, loiterNode) < maxLoiterDist) {
                        path.Add(potentialNode);
                        potentialNodes.Clear();
                    } else {
                        potentialNodes.RemoveAt(index);
                    }
                }
            }

            return new Path(path);
        }

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
                    candidatePromising.setG(mostPromising);
                    candidatePromising.setH(goal);
                    float potentialF = candidatePromising.calculateF();

                    //check this is the best route we know to the candidate
                    if (openSet.ContainsKey(candidatePromising.Node.Position) && openSet[candidatePromising.Node.Position].ValueF < potentialF && UnityEngine.Random.Range(0f,1f) > navigationAccuracy) {
                        //Not interested
                    } else if (closedSet.ContainsKey(candidatePromising.Node.Position) && closedSet[candidatePromising.Node.Position].ValueF < potentialF) {
                        //Not interested
                    } else {
                        //this is the best route to the candidate node
                        candidatePromising.setF();

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

        //Convert Dictionary Linkages to Linear Ordered List
        private static List<Node> convertParentageToList(HeuristicalNode goal) {
            List<Node> list = new List<Node>();
            list.Add(goal.Node);
            for (HeuristicalNode n = goal; n != null; n = n.Parent) {
                list.Add(n.Node);
            }
            list.Reverse();
            return list;
        }

        //Find the lowest scoring node in the provided set & dictionary.
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

        //Extension for Node providing storage and calculation for pathing heuristics
        private class HeuristicalNode {

            public float ValueG { get; set; }
            public float ValueH { get; set; }
            public float ValueF { get; set; }
            public HeuristicalNode Parent { get; set; }
            public Node Node { get; set; }

            public HeuristicalNode(Node node) {
                ValueG = 0;
                ValueH = 0;
                ValueF = float.MaxValue;
                Parent = null;
                Node = node;
            }

            //calculates and sets heurisitics
            public void setG(HeuristicalNode parentNode) {
                ValueG = calculateG(parentNode);
            }
            public void setH(HeuristicalNode goalNode) {
                ValueH = calculateH(goalNode);
            }
            public void setF() {
                ValueF = calculateF();
            }

            //calculates G (Cost Complete)
            public float calculateG(HeuristicalNode parentNode) {
                return parentNode.ValueG + distanceBetween(this.Node, parentNode.Node);
                //return distanceBetween(this.Node, startNode.Node);
            }

            //calculates H (Cost remaining [guessed])
            public float calculateH(HeuristicalNode goalNode) {
                return distanceBetween(this.Node, goalNode.Node);
            }

            //calculates F (Total Cost)
            public float calculateF() {
                return ValueG + ValueH;
            }

            //Returns position Vector as string
            public override string ToString() {
                return Node.Position.ToString() ?? "Null Node";
            }

            //Return linear distance between 2 nodes
            private static float distanceBetween(Node a, Node b) {
                return (a.Position - b.Position).magnitude;
            }

        }

    }
}

