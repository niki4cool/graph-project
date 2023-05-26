using System.Linq;

namespace GraphEditor.Models.Graph
{
    public class GraphValidator
    {
        public static bool IsValid(GraphData graph, GraphData graphClass)
        {
            var classNodes = new Dictionary<string, GraphNode>();
            foreach (var classNode in graphClass.Nodes)
            {
                classNodes.Add(classNode.Id, classNode);
            }

            foreach (var link in graph.Links)
            {
                if (!classNodes.ContainsKey(link.Target.Type) || !classNodes.ContainsKey(link.Source.Type))
                    return false;
                if (!classNodes[link.Source.Type].AdjacentNodes.Contains(classNodes[link.Target.Type]))
                    return false;                
            }
            
            return true;
        }
    }
}
