using GraphEditor.Models.CRUD;
using Microsoft.EntityFrameworkCore;

namespace GraphEditor.Models.Graph
{
    public class GraphRepository : RecordRepositoryBase<GraphRecord>
    {
        public GraphRepository(GraphDBContext context) : base(context)
        {
        }
    }
}
