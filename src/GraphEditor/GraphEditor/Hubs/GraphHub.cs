using EfCoreTest.Model.GraphModel;
using GraphEditor.CRUD;
using GraphEditor.Model.GraphModel;
using Microsoft.AspNetCore.SignalR;

namespace GraphEditor.Hubs
{
    public partial class GraphHub : Hub
    {
        private readonly IRepository<Graph> graphRepository;

        public GraphHub(IRepository<Graph> repo)
        {
            graphRepository = repo;
        }

        public override async Task OnConnectedAsync()
        {
            var graph = await GetContextGraph();
            if (graph == null)
            {
                Context.Abort();
                return;
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, graph.Name);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var graph = await GetContextGraph();
            if (graph == null)
            {
                Context.Abort();
                return;
            }
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, graph.Name);
            await base.OnDisconnectedAsync(exception);
        }

        private async Task<Graph?> GetContextGraph()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext == null)
                throw new Exception("Only http allowed");
            var id = httpContext.Request.Query["graphId"].ToString();
            return await graphRepository.Find(id);
        }
    }
}
