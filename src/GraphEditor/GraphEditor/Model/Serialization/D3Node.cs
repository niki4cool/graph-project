using GraphEditor.Model.GraphModel;

namespace GraphEditor.Model.Serialization
{
    [Serializable]
    public class D3Node
    {
        public string id;
        public string name;
        public string color;
        public string nodeClass;
        public float x, y;

        public D3Node(Node graphNode)
        {
            id = graphNode.Id;
            x = graphNode.Meta.X; y = graphNode.Meta.Y;
            name = graphNode.Meta.Name;
            color = graphNode.Meta.Color;
            nodeClass = graphNode.Meta.NodeClass;
        }
    }
}
