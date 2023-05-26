using GraphEditor.Model.GraphModel;

namespace EfCoreTest.Model.GraphModel
{
    public partial class GraphData
    {
        public bool Contains(params Node[] nodesToCheck)
        {
            foreach (var node in nodesToCheck)
                if (!Nodes.Contains(node))
                    return false;
            return true;
        }

        public bool Contains(params Edge[] edgesToCheck)
        {
            foreach (var edge in edgesToCheck)
                if (!Edges.Contains(edge))
                    return false;
            return true;
        }

        private void AssertBelonging(params Node[] nodesToCheck)
        {
            if (!Contains(nodesToCheck))
                throw new ArgumentException("Nodes does not belong to graph");
        }

        private void AssertBelonging(params Edge[] edgesToCheck)
        {
            if (!Contains(edgesToCheck))
                throw new ArgumentException("Edges does not belong to graph");
        }

    }
}
