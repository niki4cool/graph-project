using GraphEditor.DataTypes;
using Microsoft.EntityFrameworkCore;

namespace GraphEditor.Models
{
    public class GraphDBContext : DbContext
    {
        public GraphDBContext(DbContextOptions<GraphDBContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GraphRecord>()
                .OwnsOne(p => p.Data)
                .OwnsMany(p => p.Links);

            modelBuilder.Entity<GraphRecord>()
                .OwnsOne(p => p.Data)
                .OwnsMany(p => p.Nodes);

            modelBuilder.Entity<GraphRecord>()
                .OwnsOne(p => p.Data)
                .OwnsMany(p => p.Nodes)
                .OwnsOne(p => p.Meta);
        }

        public DbSet<GraphRecord> GraphRecords { get; set; } = default!;
    }
}
