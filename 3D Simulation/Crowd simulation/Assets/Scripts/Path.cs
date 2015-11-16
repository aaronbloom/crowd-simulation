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

    

}

