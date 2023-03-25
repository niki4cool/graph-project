using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GraphEditor.Models.Graph
{
    public class GraphNode
    {
        public string Id { get; set; } = string.Empty;
        public float X { get; set; }
        public float Y { get; set; }
        public GraphNodeMeta Meta { get; set; } = default!;
    }
}
