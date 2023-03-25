using GraphEditor.Models.CRUD;
using GraphEditor.Models.Graph;

namespace GraphEditor.Models.Auth.User
{
    public class UserRecord : EntityBase
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NormalizedName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public List<GraphRecord> CanEdit { get; set; } = new();
        public List<GraphRecord> CanView { get; set; } = new();
        public List<GraphRecord> Creations { get; set; } = new();
    }
}
