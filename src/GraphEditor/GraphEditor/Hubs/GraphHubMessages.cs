using GraphEditor.Model.GraphModel;
using GraphEditor.Model.Serialization;
using Microsoft.AspNetCore.SignalR;

namespace GraphEditor.Hubs
{
    public partial class GraphHub : Hub
    {
        public async Task CreateNode(CreateNodeRequest request)
        {
            var graph = (await GetContextGraph())!;
            var node = graph.Data.CreateNode();
            node.Meta = request.Meta;
            await graphRepository.Update(graph);
            var json = GraphSerializer.GraphToJson(graph);
            await Clients.Group(graph.Name)
                  .SendCoreAsync("OnGraphUpdate", new object[] { json });
        }

        public async Task RemoveNode(RemoveNodeRequest request)
        {
            var graph = (await GetContextGraph())!;
            var node = graph.Data.FindNode(request.NodeId);
            graph.Data.RemoveNode(node!);
            await graphRepository.Update(graph);
            var json = GraphSerializer.GraphToJson(graph);
            await Clients.Group(graph.Name)
                  .SendCoreAsync("OnGraphUpdate", new object[] { json });
        }

        public async Task Connect(ConnectRequest request)
        {
            var graph = (await GetContextGraph())!;
            var from = graph.Data.FindNode(request.FromId);
            var to = graph.Data.FindNode(request.ToId);
            if (!graph.CanConnect(from!, to!))
                return;
            graph.Data.Connect(from!, to!);
            await graphRepository.Update(graph);
            var json = GraphSerializer.GraphToJson(graph);
            await Clients.Group(graph.Name)
                  .SendCoreAsync("OnGraphUpdate", new object[] { json });
        }

        public async Task Disconnect(DisconnectRequest request)
        {
            var graph = (await GetContextGraph())!;
            var from = graph.Data.FindNode(request.FromId);
            var to = graph.Data.FindNode(request.FromId);
            var edge = graph.Data.FindEdge(from!, to!);
            graph.Data.Disconnect(edge!);
            await graphRepository.Update(graph);
            var json = GraphSerializer.GraphToJson(graph);
            await Clients.Group(graph.Name)
                  .SendCoreAsync("OnGraphUpdate", new object[] { json });
        }

        public async Task RemoveEdge(RemoveEdgeRequest request)
        {
            var graph = (await GetContextGraph())!;
            var edge = graph.Data.FindEdge(request.EdgeId);
            graph.Data.Disconnect(edge!);
            await graphRepository.Update(graph);
            var json = GraphSerializer.GraphToJson(graph);
            await Clients.Group(graph.Name)
                  .SendCoreAsync("OnGraphUpdate", new object[] { json });
        }

        public async Task SetNodeMeta(SetNodeMetaRequest request)
        {
            var graph = (await GetContextGraph())!;
            var node = graph.Data.FindNode(request.NodeId);
            node!.Meta = request.Meta;
            await graphRepository.Update(graph);
            var json = GraphSerializer.GraphToJson(graph);
            await Clients.Group(graph.Name)
                  .SendCoreAsync("OnGraphUpdate", new object[] { json });
        }

        public async Task SetEdgeMeta(SetEdgeMetaRequest request)
        {
            var graph = (await GetContextGraph())!;
            var edge = graph.Data.FindEdge(request.EdgeId);
            edge!.Meta = request.Meta;
            await graphRepository.Update(graph);
            var json = GraphSerializer.GraphToJson(graph);
            await Clients.Group(graph.Name)
                  .SendCoreAsync("OnGraphUpdate", new object[] { json });
        }

        public async Task RequestUpdate()
        {
            var graph = (await GetContextGraph())!;
            var json = GraphSerializer.GraphToJson(graph);
            await Clients.Caller.SendCoreAsync("OnGraphUpdate", new object[] { json });
        }
    }
}
