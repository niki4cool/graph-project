﻿using GraphEditor.Models.Auth.User;
using GraphEditor.Models.CRUD;

namespace GraphEditor.Models.Graph
{
    public class GraphRecord : EntityBase
    {
        public string Name = string.Empty;

        public GraphRecord GraphClass;

        public GraphData Data { get; set; } = new();

        public GraphType Type { get; set; }

        public UserRecord Creator { get; set; } = new();
        public List<UserRecord> Editors { get; set; } = new();
        public List<UserRecord> Viewers { get; set; } = new();

        public string EditRole => $"Graph-{Id}-Editor";
        public string ViewRole => $"Graph-{Id}-Viewer";

        public GraphRecord(string id) : base(id) { }
    }

    public enum GraphType
    {
        Regular, Class
    }
}
