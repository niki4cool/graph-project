﻿using GraphEditor.Models.Auth.User;
using GraphEditor.Models.CRUD;

namespace GraphEditor.Models.Graph
{
    public class GraphRecord : EntityBase
    {
        public string Name = string.Empty;
        public List<GraphLink> Links { get; set; } = new();
        public List<GraphNode> Nodes { get; set; } = new();

        public string EditRole => $"Graph-{Id}-Editor";
        public string ViewRole => $"Graph-{Id}-Viewer";

        public GraphRecord(string id) : base(id) { }
    }
}
