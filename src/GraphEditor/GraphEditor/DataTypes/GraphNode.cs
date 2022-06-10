using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GraphEditor.DataTypes
{   
    public class GraphNode
    {
        public string Id { get; set; } = String.Empty;
        public float X { get; set; }
        public float Y { get; set; }

        public Meta Meta { get; set; } = default!;
    }
}
