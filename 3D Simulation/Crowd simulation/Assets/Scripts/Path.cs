using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Path : Graph {

    //Constructs Path from ordered node list
    public Path(List<Node> nodes) : base(nodes) {
    }

    //TODO: Refactor out. Replace with Graph.Nodes    #!#    #!#    #!#    #!#
    public List<Node> RemainingNodes {
        get {
            return Nodes;
        }
    }

    public static Path Navigate(Graph graph, Node startNode, Node goalNode) {

        List<Node> closedSet = new List<Node>();
        List<Node> openSet = new List<Node>();

        //Special case for startNode
        openSet.Add(startNode);

        //Extension fields for Node
        Dictionary<Node, float> fScores = new Dictionary<Node, float>();
        Dictionary<Node, float> gScores = new Dictionary<Node, float>();
        Dictionary<Node, float> hScores = new Dictionary<Node, float>();

        foreach(Node node in graph.Nodes) {
            fScores.Add(node, 1);
            gScores.Add(node, 1);
            hScores.Add(node, 1);
        }
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
                    parents.Add(candidatePromisingNode, mostPromisingNode);
                    return new Path(convertParentageToList(parents, goalNode));
                }
                //calculate heuristics for candiate node
                float candidateG = calculateG(mostPromisingNode, candidatePromisingNode, gScores);
                float candidateH = calculateH(goalNode, candidatePromisingNode);
                float candidateF = calculateF(candidatePromisingNode, gScores, hScores);
                //check this is the best route we know to the candidate
                if (isBetterOpenPath(openSet, fScores, candidatePromisingNode, candidateF)) {
                    //skip, already better path for this node in open list
                } else if (isBetterClosedPath(closedSet, fScores, candidatePromisingNode, candidateF)) {
                    //skip, already better path for this node in closed list
                } else {
                    //this is the best route to the candidate node
                    //remove old entries
                    if (parents.ContainsKey(candidatePromisingNode)) parents.Remove(candidatePromisingNode);
                    //add parent for pathing
                    parents.Add(candidatePromisingNode, mostPromisingNode);
                    //add to open set
                    openSet.Add(candidatePromisingNode);
                    //update stored node heuristics
                    gScores[candidatePromisingNode] = candidateG;
                    hScores[candidatePromisingNode] = candidateH;
                    fScores[candidatePromisingNode] = candidateF;
                }
            }
            //all candidates processed, close node
            closedSet.Add(mostPromisingNode);
        }
        throw new InvalidOperationException("Specified goalNode was not in the same navigational Graph as startNode");
    }

    private static bool isBetterOpenPath(List<Node> openSet, Dictionary<Node, float> fScores, Node candidatePromisingNode, float candidateF) {
        return openSet.Contains(candidatePromisingNode) && fScores[candidatePromisingNode] < candidateF;
    }
    private static bool isBetterClosedPath(List<Node> closedSet, Dictionary<Node, float> fScores, Node candidatePromisingNode, float candidateF) {
        return closedSet.Contains(candidatePromisingNode) && fScores[candidatePromisingNode] < candidateF;
    }

    private static float calculateG(Node mostPromisingNode, Node candidatePromisingNode, Dictionary<Node, float> gScores) {
        return gScores[mostPromisingNode] + distanceBetweenNodes(candidatePromisingNode, mostPromisingNode);
    }

    private static float calculateH(Node goalNode, Node candidatePromisingNode) {
        return distanceBetweenNodes(goalNode, candidatePromisingNode);
    }

    private static float calculateF(Node candidatePromisingNode, Dictionary<Node, float> gScores, Dictionary<Node, float> hScores) {
        return gScores[candidatePromisingNode] + hScores[candidatePromisingNode];
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

