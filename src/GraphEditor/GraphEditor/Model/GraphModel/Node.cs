using GraphEditor.CRUD;

namespace GraphEditor.Model.GraphModel
{
    public class Node : EntityBase
    {
        public NodeMeta Meta { get; set; } = new();

        public Node() : base() { }
    }
}
