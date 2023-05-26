using GraphEditor.CRUD;
using GraphEditor.Model.GraphModel;

namespace GraphEditor.Model
{
    public class User : EntityBase
    {
        public string Name { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public List<Graph> CanEdit { get; } = new();
        public List<Graph> CanView { get; } = new();
        public List<Graph> Creations { get; } = new();
    }
}
