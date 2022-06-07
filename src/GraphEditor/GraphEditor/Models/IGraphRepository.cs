namespace GraphEditor.Models
{
    public interface IGraphRepository
    {
        Task<GraphRecord?> Get(string id);
        Task Create(GraphRecord item);
        Task Update(GraphRecord item);
        Task Delete(string id);
    }
}
