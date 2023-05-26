using EfCoreTest.Model.GraphModel;
using GraphEditor.CRUD;
using GraphEditor.Model;
using GraphEditor.Model.GraphModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

public class GraphRepository : IRepository<Graph>
{
    private readonly GraphDBContext context;

    public GraphRepository(GraphDBContext context)
    {
        this.context = context;
    }

    private IIncludableQueryable<Graph, User> QueryWithInclusions()
    {   
        return context.Graphs.Include(e => e.CanBeViewedBy)
                             .Include(e => e.CanBeEditedBy)
                             .Include(e => e.Data.Nodes)
                             .Include(e => e.Data.Edges)
                             .Include(e => e.ClassGraph.Data.Nodes)
                             .Include(e => e.ClassGraph.Data.Edges)
                             .Include(e => e.Creator);
    }

    public async Task Add(Graph item)
    {
        context.Graphs.Add(item);
        await context.SaveChangesAsync();
    }

    public async Task Delete(string id)
    {
        context.Graphs.Remove(await Get(id));
        await context.SaveChangesAsync();
    }

    public async Task<Graph?> Find(string name)
    {
        var graph = await QueryWithInclusions().Where(e => e.Name == name)
                                               .FirstOrDefaultAsync();
        return graph;
    }

    public async Task<Graph> Get(string id)
    {
        var graph = await Find(id);
        return graph!;
    }

    public async Task Update(Graph item)
    {
        context.Graphs.Update(item);
        await context.SaveChangesAsync();
    }
}