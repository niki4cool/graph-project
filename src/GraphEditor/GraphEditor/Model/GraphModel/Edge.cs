using GraphEditor.CRUD;

namespace GraphEditor.Model.GraphModel
{
    public class Edge : EntityBase
    {
        public Node From { get; set; } = new();
        public Node To { get; set; } = new();
        public EdgeMeta Meta { get; set; } = new();

        public Edge() : base() { }        
    }
}
