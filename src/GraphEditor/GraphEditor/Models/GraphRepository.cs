using Microsoft.EntityFrameworkCore.SqlServer;

namespace GraphEditor.Models
{
    public class GraphRepository : IGraphRepository
    {
        private readonly GraphDBContext context;

        public GraphRepository(GraphDBContext context)
        {
            this.context = context;
        }

        public async Task Create(GraphRecord item)
        {
            await context.GraphRecords.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public async Task<GraphRecord?> Get(string Id)
        {
            return await context.GraphRecords.FindAsync(Id);
        }

        public async Task Update(GraphRecord updatedRecord)
        {
            var graph = await Get(updatedRecord.Id);
            if (graph == null)
                throw new KeyNotFoundException(nameof(updatedRecord.Id));
            graph.Data = updatedRecord.Data;
            context.GraphRecords.Update(graph);
            await context.SaveChangesAsync();
        }

        public async Task Delete(string Id)
        {
            var graph = await Get(Id);
            if (graph == null)
                return;
            context.GraphRecords.Remove(graph);
            await context.SaveChangesAsync();
        }
    }
}
