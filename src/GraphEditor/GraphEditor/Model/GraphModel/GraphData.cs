using GraphEditor.CRUD;
using GraphEditor.Model.GraphModel;
using GraphEditor.Model.Serialization;

namespace EfCoreTest.Model.GraphModel
{
    public partial class GraphData : EntityBase
    {
        public List<Node> Nodes { get; } = new();
        public List<Edge> Edges { get; } = new();

        public GraphData() : base() { }

        public Node CreateNode()
        {
            var node = new Node();
            Nodes.Add(node);
            return node;
        }

        public Node? FindNode(string id)
        {
            return Nodes.Where(e => e.Id == id).FirstOrDefault();
        }

        public void RemoveNode(Node node)
        {
            AssertBelonging(node);
            var incidentEdges = IncidentEdges(node!).ToList();
            foreach (var edge in incidentEdges)
                Edges.Remove(edge);
            Nodes.Remove(node);
        }

        public Edge? FindEdge(string id)
        {
            return Edges.Where(e => e.Id == id).FirstOrDefault();
        }

        public Edge? FindEdge(Node from, Node to)
        {
            AssertBelonging(from, to);
            var validEdges = Edges.Where(e => e.From == from && e.To == to).ToArray();
            if (validEdges.Length == 0)
                return null;
            if (validEdges.Length == 1)
                return validEdges[0];
            throw new Exception("Invalid graph");
        }

        public Edge Connect(Node from, Node to)
        {
            AssertBelonging(from, to);
            var edge = FindEdge(from, to);
            if (edge != null)
                return edge;
            edge = new Edge()
            {
                From = from,
                To = to
            };
            Edges.Add(edge);
            return edge;
        }

        public void Disconnect(Node from, Node to)
        {
            var edge = FindEdge(from, to);
            if (edge == null)
                return;
            Disconnect(edge);
        }

        public void Disconnect(Edge edge)
        {
            AssertBelonging(edge);
            Edges.Remove(edge);
        }

        public IEnumerable<Edge> PointsFrom(Node node)
        {
            AssertBelonging(node);
            return Edges.Where(e => e.From == node);
        }

        public IEnumerable<Edge> PointsTo(Node node)
        {
            AssertBelonging(node);
            return Edges.Where(e => e.To == node);
        }

        public IEnumerable<Edge> IncidentEdges(Node node)
        {
            AssertBelonging(node);
            return Edges.Where(e => e.To == node || e.From == node);
        }
    }
}
