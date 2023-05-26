using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GraphEditor.Models.Graph
{
    public class GraphLink
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public GraphNode? Source { get; set; } = null;
        public GraphNode? Target { get; set; } = null;
    }
}
