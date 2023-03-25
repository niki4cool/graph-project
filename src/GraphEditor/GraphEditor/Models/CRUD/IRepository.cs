using GraphEditor.Models.Graph;

namespace GraphEditor.Models.CRUD
{
    public interface IRepository<RecordT> where RecordT : EntityBase
    {
        Task<IQueryable<RecordT>> AsQueryable();
        Task<RecordT> Get(string id);
        Task<RecordT?> Find(string id);
        Task Add(RecordT item);
        Task Update(RecordT item);
        Task Delete(string id);
    }
}
