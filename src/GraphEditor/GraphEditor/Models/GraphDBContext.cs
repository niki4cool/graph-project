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
            modelBuilder.Entity<GraphRecord>().HasOne(p => p.Data);            
            modelBuilder.Entity<GraphRecord>().HasMany(p => p.Editors)
                                              .WithMany(p => p.CanEdit)
                                              .UsingEntity(e => e.ToTable("Editors"));
            modelBuilder.Entity<GraphRecord>().HasMany(p => p.Viewers)
                                              .WithMany(p => p.CanView)
                                              .UsingEntity(e => e.ToTable("Viewers"));
            modelBuilder.Entity<GraphRecord>().HasOne(p => p.Creator)
                                              .WithMany(p => p.Creations);

            modelBuilder.Entity<GraphData>().HasKey(p => p.Id);
            modelBuilder.Entity<GraphData>().HasMany(p => p.Links);
            modelBuilder.Entity<GraphData>().HasMany(p => p.Nodes);

            modelBuilder.Entity<GraphNode>().HasKey(p => p.Id);
            modelBuilder.Entity<GraphNode>().OwnsOne(p => p.Meta);

            modelBuilder.Entity<GraphLink>().HasKey(p => p.Id);

            //user
            modelBuilder.Entity<UserRecord>().HasKey(p => p.Id);
        }

        public DbSet<GraphRecord> GraphRecords { get; set; } = default!;
        public DbSet<GraphRecord> UserRecords { get; set; } = default!;
    }
}
