using EfCoreTest.Model.GraphModel;
using GraphEditor.CRUD;

namespace GraphEditor.Model.GraphModel
{
    public class Graph : EntityBase
    {
        public GraphData Data { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public GraphType GraphType { get; set; } = GraphType.Free;
        public Graph? ClassGraph { get; set; }

        public User Creator { get; set; } = new();
        public List<User> CanBeViewedBy { get; } = new();
        public List<User> CanBeEditedBy { get; } = new();

        public string ViewRole => $"Graph_{Id}_Viewer";
        public string EditRole => $"Graph_{Id}_Editor";

        public Graph() : base() {}

        public Graph(string name,
                     User creator,
                     GraphType graphType,
                     Graph? classGraph) : base()
        {
            if (graphType == GraphType.InstanceGraph && classGraph == null)
                throw new ArgumentNullException(nameof(classGraph));

            Data = new();
            Name = name;
            GraphType = graphType;
            ClassGraph = classGraph;
            Creator = creator;
            CanBeViewedBy = new();
            CanBeEditedBy = new();
        }

        public bool CanConnect(Node from, Node to)
        {
            if (!Data.Contains(from, to))
                return false;
            if (GraphType == GraphType.Free ||
                GraphType == GraphType.ClassGraph)
                return true;

            if (ClassGraph == null)
                throw new Exception("Regular graph whisout a class");

            var fromClass = ClassGraph.Data.Nodes
                .Where(n => n.Meta.Name == from.Meta.NodeClass)
                .FirstOrDefault();
            var toClass = ClassGraph.Data.Nodes
                .Where(n => n.Meta.Name == to.Meta.NodeClass)
                .FirstOrDefault();

            if (fromClass == null || toClass == null)
                return false;

            return ClassGraph.Data.PointsFrom(fromClass).Select(e => e.To).Contains(toClass);
        }
    }

    public enum GraphType
    {
        InstanceGraph, ClassGraph, Free
    }
}
