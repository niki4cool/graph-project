namespace GraphEditor.CRUD
{
    public interface IRepository<RecordT> where RecordT : EntityBase
    {
        Task<RecordT> Get(string id);
        Task<RecordT?> Find(string id);
        Task Add(RecordT item);
        Task Update(RecordT item);
        Task Delete(string id);
    }
}
