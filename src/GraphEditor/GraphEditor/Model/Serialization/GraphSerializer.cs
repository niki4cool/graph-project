using System.Text.Json;
using GraphEditor.Model.GraphModel;

namespace GraphEditor.Model.Serialization
{
    public class GraphSerializer
    {
        public static string GraphToJson(Graph graphRecord)
        {
            var result = new D3Graph(graphRecord);
            var options = new JsonSerializerOptions();
            options.IncludeFields = true;
            return JsonSerializer.Serialize(result, options);
        }
    }
}