namespace GraphEditor.Controllers.GraphController
{
    public class CreateGraphRequest
    {
        public string GraphType { get; set; } = String.Empty;
        public string? GraphClassId { get; set; }
    }
}
