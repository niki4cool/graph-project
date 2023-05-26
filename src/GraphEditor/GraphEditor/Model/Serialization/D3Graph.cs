using GraphEditor.Model.GraphModel;

namespace GraphEditor.Model.Serialization
{
    [Serializable]
    public class D3Graph
    {
        public string id;
        public string name;
        public string graphType;
        public D3Data data;

        public D3Graph(Graph graphRecord)
        {
            id = graphRecord.Id;
            name = graphRecord.Name;
            graphType = Enum.GetName(graphRecord.GraphType)!;
            data = new D3Data(graphRecord.Data);
        }
    }
}
