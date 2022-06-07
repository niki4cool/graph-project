using GraphEditor.DataTypes;

namespace GraphEditor.Models
{
    public class GraphRecord
    {
        public string Id { get; set; }
        public GraphData Data { get; set; }

        public GraphRecord(string id, GraphData data)
        {
            Id = id;
            Data = data;
        }

        public GraphRecord()
        {
            Id = Guid.NewGuid().ToString();
            Data = new GraphData();
        }
    }
}
