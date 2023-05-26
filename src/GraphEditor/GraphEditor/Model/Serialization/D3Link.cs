using GraphEditor.Model.GraphModel;

namespace GraphEditor.Model.Serialization
{
    [Serializable]
    public class D3Link
    {
        public string id;
        public string source;
        public string target;
        public float value;

        public D3Link(Edge graphEdge)
        {
            id = graphEdge.Id;
            source = graphEdge.From.Id;
            target = graphEdge.To.Id;
            value = graphEdge.Meta.Value;
        }
    }
}
