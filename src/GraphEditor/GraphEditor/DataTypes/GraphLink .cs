using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GraphEditor.DataTypes
{   
    public class GraphLink
    {
        public string Source { get; set; } = String.Empty;
        public string Target { get; set; } = String.Empty;
    }
}
