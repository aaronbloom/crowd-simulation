namespace Assets.Scripts.Environment.Navigation {
    public class Transition {

        public Node[] Nodes { get; private set; }

        public Transition (Node one, Node two) {
            Nodes = new Node[2] {one, two};
        }
    }
}
