using GraphEditor.Model.GraphModel;

namespace GraphEditor.Hubs
{
    public class CreateNodeRequest
    {
        public NodeMeta Meta { get; set; } = new();
    }
    public class SetNodeMetaRequest
    {
        public string NodeId { get; set; } = String.Empty;
        public NodeMeta Meta { get; set; } = new();
    }

    public class RemoveNodeRequest
    {
        public string NodeId { get; set; } = String.Empty;
    }

    public class ConnectRequest
    {
        public string FromId { get; set; } = String.Empty;
        public string ToId { get; set; } = String.Empty;
    }

    public class DisconnectRequest
    {
        public string FromId { get; set; } = String.Empty;
        public string ToId { get; set; } = String.Empty;
    }

    public class RemoveEdgeRequest
    {
        public string EdgeId { get; set; } = String.Empty;
    }

    public class SetEdgeMetaRequest
    {
        public string EdgeId { get; set; } = String.Empty;
        public EdgeMeta Meta { get; set; } = new();
    }
}