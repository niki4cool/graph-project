using GraphEditor.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace GraphEditor.DataTypes
{
    public class GraphData
    {        
        public List<GraphNode> Nodes { get; set; } = new List<GraphNode>();
        public List<GraphLink> Links { get; set; } = new List<GraphLink>();
    }
}
