using GraphEditor.Models.Auth.User;
using GraphEditor.Models.Graph;
using Microsoft.EntityFrameworkCore;

namespace GraphEditor.Models
{
    public class GraphDBContext : DbContext
    {
        public GraphDBContext(DbContextOptions<GraphDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //graph
            modelBuilder.Entity<GraphRecord>().HasKey(p => p.Id);
            modelBuilder.Entity<GraphRecord>().HasMany(p => p.Links);
            modelBuilder.Entity<GraphRecord>().HasMany(p => p.Nodes);

            modelBuilder.Entity<GraphNode>().HasKey(p => p.Id);
            modelBuilder.Entity<GraphNode>().OwnsOne(p => p.Meta);

            modelBuilder.Entity<GraphLink>().HasKey(p => p.Id);
            modelBuilder.Entity<GraphLink>().HasOne(p => p.Source);
            modelBuilder.Entity<GraphLink>().HasOne(p => p.Target);


            //user
            modelBuilder.Entity<UserRecord>().HasKey(p => p.Id);
            modelBuilder.Entity<UserRecord>().HasMany(p => p.CanEdit);
        }

        public DbSet<GraphRecord> GraphRecords { get; set; } = default!;
        public DbSet<GraphRecord> UserRecords { get; set; } = default!;
    }
}
