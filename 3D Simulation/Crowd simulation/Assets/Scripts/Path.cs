using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Path {

    private List<Node> nodes;
    //Removes the next node, returning it
    public Node PopNode {
        get {
            Node n = PeakNode;
            nodes.Remove(n);
            return n;
        }
    }
    //Returns the next node
    public Node PeakNode {
        get {
            return nodes[0];
        }
    }
    //Returns the Goal Node
    public Node GoalNode {
        get {
            return nodes[nodes.Count - 1];
        }
    }

    //Constructs Path from ordered node list
    public Path(List<Node> nodes) {
        this.nodes = nodes;
    }

    public static Path Navigate(Node startNode, Node goalNode) {

        List<Node> closedSet = new List<Node>();
        List<Node> openSet = new List<Node>();

        //Special case for startNode
        openSet.Add(startNode);

        //Extension fields for Node
        Dictionary<Node, float> fScores = new Dictionary<Node, float>();
        Dictionary<Node, float> gScores = new Dictionary<Node, float>();
        Dictionary<Node, float> hScores = new Dictionary<Node, float>();

        // <Node,Node> <child,parent>
        Dictionary<Node, Node> parents = new Dictionary<Node, Node>();  

        //Special case for startNode
        parents.Add(startNode, null);     

        //Process Open Set
        while (openSet.Count > 0) {
            //Choose most promising node
            Node mostPromisingNode = lowestFScoreInOpenSet(fScores, openSet); //needs renaming
            //Remove most promising node from open set
            openSet.Remove(mostPromisingNode);
            //Process Nodes which most Promising Node connects to
            foreach (var candidatePromisingNode in mostPromisingNode.TransitionsTo) {
                //See if we're at our destination
                if (candidatePromisingNode == goalNode) {
                    //return the path to the goal from start
                    return new Path(convertParentageToList(parents, goalNode));
                }
                //calculate heuristics for candiate node
                float g = gScores[mostPromisingNode] + distanceBetweenNodes(candidatePromisingNode, mostPromisingNode);
                float h = distanceBetweenNodes(goalNode, candidatePromisingNode);
                float f = gScores[candidatePromisingNode] + hScores[candidatePromisingNode];
                //check this is the best route we know to the candidate
                if (openSet.Contains(candidatePromisingNode) && fScores[candidatePromisingNode] < f) {
                    //skip, already better path for this node in open list
                } else if (closedSet.Contains(candidatePromisingNode) && fScores[candidatePromisingNode] < f) {
                    //skip, already better path for this node in closed list
                } else {
                    //this is the best route to the candidate node
                    //remove old entries
                    if (!parents.ContainsKey(candidatePromisingNode)) parents.Remove(candidatePromisingNode);
                    //add parent for pathing
                    parents.Add(candidatePromisingNode, mostPromisingNode);
                    //add to open set
                    openSet.Add(candidatePromisingNode);
                    //update stored node heuristics
                    gScores[candidatePromisingNode] = g;
                    hScores[candidatePromisingNode] = h;
                    fScores[candidatePromisingNode] = f;
                }
            }
            //all candidates processed, close node
            closedSet.Add(mostPromisingNode);
        }
        throw new Exception("Could not find goal");
    }

    //Convert Dictionary Linkages to Linear Ordered List
    private static List<Node> convertParentageToList(Dictionary<Node, Node> childParent, Node goal) {
        List<Node> list = new List<Node>();
        list.Add(goal);
        for(Node n = goal; n != null; n = childParent[n]) {
            list.Add(n);
        }
        list.Reverse();
        return list;
    }

    //Return linear distance between 2 nodes
    private static float distanceBetweenNodes(Node a, Node b) {
        return (a.Position - b.Position).magnitude;
    }

    //Find the lowest scoring node in the provided set & dictionary.
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

