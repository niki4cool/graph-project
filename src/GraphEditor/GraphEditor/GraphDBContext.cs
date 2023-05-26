using EfCoreTest.Model.GraphModel;
using GraphEditor.Model.GraphModel;
using Microsoft.EntityFrameworkCore;

namespace GraphEditor.Model
{
    public class GraphDBContext : DbContext
    {
        public GraphDBContext(DbContextOptions<GraphDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var user = modelBuilder.Entity<User>();
            user.HasKey(e => e.Id);
            user.HasMany(e => e.CanView).WithMany(e => e.CanBeViewedBy).UsingEntity("GraphUser_View");
            user.HasMany(e => e.CanEdit).WithMany(e => e.CanBeEditedBy).UsingEntity("GraphUser_Edit");
            user.HasMany(e => e.Creations).WithOne(e => e.Creator);

            modelBuilder.Entity<Graph>().HasKey(e => e.Id);

            modelBuilder.Entity<GraphData>().HasKey(e => e.Id);

            var edge = modelBuilder.Entity<Edge>();
            edge.HasKey(e => e.Id);
            edge.OwnsOne(e => e.Meta);

            var node = modelBuilder.Entity<Node>();
            node.HasKey(e => e.Id);
            node.OwnsOne(e => e.Meta);

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Graph> Graphs { get; set; } = default!;
        public DbSet<User> Users { get; set; } = default!;
    }
}
