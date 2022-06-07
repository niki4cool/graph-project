using GraphEditor.DataTypes;
using GraphEditor.Models;
using Microsoft.AspNetCore.SignalR;

namespace GraphEditor.Hubs
{
    public class GraphHub : Hub
    {
        private readonly IGraphRepository graphRepository;

        public GraphHub(IGraphRepository graphRepository)
        {
            this.graphRepository = graphRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var graph = await GetContextGraph();
            if (graph == null)
                Context.Abort();
            else
                await Groups.AddToGroupAsync(Context.ConnectionId, graph.Id);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var graph = await GetContextGraph();
            if (graph == null)
                Context.Abort();
            else
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, graph.Id);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task UpdateGraph(GraphData data)
        {
            var graph = await GetContextGraph();
            if (graph == null)
            {
                Context.Abort();
                return;
            }
            graph.Data = data;
            await graphRepository.Update(graph);
            await Clients.GroupExcept(graph.Id, Context.ConnectionId)
                  .SendCoreAsync("OnGraphUpdate", new object[] { data });
        }

        public async Task RequestUpdate()
        {
            var graph = await GetContextGraph();
            if (graph == null)
                Context.Abort();
            else
                await Clients.Caller
                          .SendCoreAsync("OnGraphUpdate", new object[] { graph.Data });
        }

        private async Task<GraphRecord?> GetContextGraph()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext == null)
                throw new Exception("Only http allowed");
            var id = httpContext.Request.Query["graphId"].ToString();
            return await graphRepository.Get(id);
        }
    }
}
