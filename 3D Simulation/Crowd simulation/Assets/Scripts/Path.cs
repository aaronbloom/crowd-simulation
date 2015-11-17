using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Path {

    private List<Node> nodes;
    public Node NextNode {
        get {
            return nodes[0];
        }
    }
    public Node GoalNode {
        get {
            return nodes[nodes.Count - 1];
        }
    }

    public Path(List<Node> nodes) {
        this.nodes = nodes;
    }

    public static Path Navigate(Node startNode, Node goalNode) {

        List<Node> closedSet = new List<Node>();
        List<Node> openSet = new List<Node>();
        //came from?
        openSet.Add(startNode);

        Dictionary<Node, float> fScores = new Dictionary<Node, float>();
        Dictionary<Node, float> gScores = new Dictionary<Node, float>();
        Dictionary<Node, float> hScores = new Dictionary<Node, float>();
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();  // <Node,Node> <child,parent>
        parents.Add(startNode, null);     

        while (openSet.Count > 0) {
            Node currentNode = lowestFScoreInOpenSet(fScores, openSet); //needs renaming
            openSet.Remove(currentNode);
            foreach (var successorNode in currentNode.TransitionsTo) {
                if (successorNode == goalNode) {
                    return new Path(convertParentageToList(parents, goalNode));
                }
                float g = gScores[currentNode] + distanceBetweenNodes(successorNode, currentNode);
                float h = distanceBetweenNodes(goalNode, successorNode);
                float f = gScores[successorNode] + hScores[successorNode];

                if (openSet.Contains(successorNode) && fScores[successorNode] < f) {
                    //skip, already better path for this node in open list
                } else if (closedSet.Contains(successorNode) && fScores[successorNode] < f) {
                    //skip, already better path for this node in closed list
                } else {
                    if (!parents.ContainsKey(successorNode)) parents.Remove(successorNode);
                    parents.Add(successorNode, currentNode);
                    openSet.Add(successorNode);
                    gScores[successorNode] = g;
                    hScores[successorNode] = h;
                    fScores[successorNode] = f;
                }
            }
            closedSet.Add(currentNode);
        }
        return new Path(convertParentageToList(parents, goalNode));
    }

    private static List<Node> convertParentageToList(Dictionary<Node, Node> childParent, Node goal) {
        List<Node> list = new List<Node>();
        list.Add(goal);
        for(Node n = goal; n != null; n = childParent[n]) {
            list.Add(n);
        }
        list.Reverse();
        return list;
    }

    private static float distanceBetweenNodes(Node a, Node b) {
        return (a.Position - b.Position).magnitude;
    }

    private static Node lowestFScoreInOpenSet(Dictionary<Node, float> scores, List<Node> set) {
        float lowest = float.MaxValue;
        Node winningNode = null;
        foreach (var node in set) {
            var value = scores[node];
            if (value < lowest) {
                lowest = value;
                winningNode = node;
            }
        }
        return winningNode;
    }

}

