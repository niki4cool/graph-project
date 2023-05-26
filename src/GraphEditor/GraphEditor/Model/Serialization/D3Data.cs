using EfCoreTest.Model.GraphModel;

namespace GraphEditor.Model.Serialization
{
    [Serializable]
    public class D3Data
    {
        public List<D3Node> nodes;
        public List<D3Link> links;

        public D3Data(GraphData graphData)
        {
            nodes = new List<D3Node>(graphData.Nodes.Select(n => new D3Node(n)));
            links = new List<D3Link>(graphData.Edges.Select(l => new D3Link(l)));
        }
    }
}
