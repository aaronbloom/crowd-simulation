namespace Assets.Scripts.Environment.Navigation {

    //A class to represent a connection between two nodes
    public class Transition {

        public Node[] Nodes { get; private set; }

        /// <summary>
        /// Creates transistion between Node <paramref name="one"/> and Node <paramref name="two"/>
        /// </summary>
        /// <param name="one">first node</param>
        /// <param name="two">second node</param>
        public Transition (Node one, Node two) {
            Nodes = new Node[2] {one, two};
        }
    }
}
