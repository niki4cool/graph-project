using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace GraphEditor.CRUD
{
    public abstract class RecordRepositoryBase<RecordT> : IRepository<RecordT> where RecordT : EntityBase
    {
        private readonly DbContext context;
        private readonly DbSet<RecordT> records;

        public RecordRepositoryBase(DbContext context)
        {
            this.context = context;
            records = context.Set<RecordT>();
        }

        public virtual async Task Add(RecordT item)
        {
            await records.AddAsync(item);
            await context.SaveChangesAsync();
        }

        public virtual async Task Delete(string id)
        {
            var entity = await Get(id);
            records.Remove(entity);
            await context.SaveChangesAsync();
        }

        public virtual async Task<RecordT> Get(string id)
        {
            var entity = await records.FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException(nameof(id));
            return entity;
        }

        public virtual async Task Update(RecordT item)
        {
            await Get(item.Id);
            records.Update(item);
            await context.SaveChangesAsync();
        }

        public async Task<RecordT?> Find(string id)
        {
            var entity = await records.FindAsync(id);
            return entity;
        }
    }
}
