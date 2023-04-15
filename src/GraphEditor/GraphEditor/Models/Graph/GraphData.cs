namespace GraphEditor.Models.Graph
{
    public class GraphData
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public List<GraphLink> Links { get; set; } = new();
        public List<GraphNode> Nodes { get; set; } = new();
    }
}
