using GraphEditor.Models.Auth;
using GraphEditor.Models.CRUD;
using GraphEditor.Models.Graph;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace GraphEditor.Hubs
{
    public class GraphHub : Hub
    {
        private readonly IAuthorizationService authorizationService;
        private readonly IRepository<GraphRecord> graphRepository;

        public GraphHub(IAuthorizationService auth,
                        IRepository<GraphRecord> repo)
        {
            authorizationService = auth;
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

            await Groups.AddToGroupAsync(Context.ConnectionId, graph.Id);
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

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, graph.Id);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task UpdateGraph(GraphData newData)
        {
            var graph = await GetContextGraph();
            var isValid = graph != null
                       && await CanEdit(graph);

            if (!isValid)
            {
                Context.Abort();
                return;
            }
            graph.Data.Links.Clear();
            graph.Data.Links.AddRange(newData.Links);
            graph.Data.Nodes.Clear();
            graph.Data.Nodes.AddRange(newData.Nodes);
            await graphRepository.Update(graph);
            await Clients.GroupExcept(graph.Id, Context.ConnectionId)
                  .SendCoreAsync("OnGraphUpdate", new object[] { newData });
        }

        public async Task RequestUpdate()
        {
            var graph = await GetContextGraph();
            var isValid = graph != null
                       && await CanView(graph);

            if (!isValid)
            {
                Context.Abort();
                return;
            }

            await Clients.Caller
                          .SendCoreAsync("OnGraphUpdate", new object[] { graph.Data });
        }

        private async Task<bool> CanEdit(GraphRecord graph) //TODO string token
        {
            //var authorizationResult = await authorizationService
            //    .AuthorizeAsync(Context.User, graph, Operations.Update);
            //return authorizationResult.Succeeded;
            return true;
        }

        private async Task<bool> CanView(GraphRecord graph)
        {
            //var authorizationResult = await authorizationService
            //    .AuthorizeAsync(Context.User, graph, Operations.Read);
            //return authorizationResult.Succeeded;
            return true;
        }


        private async Task<GraphRecord?> GetContextGraph()
        {
            var httpContext = Context.GetHttpContext();
            if (httpContext == null)
                throw new Exception("Only http allowed");
            var id = httpContext.Request.Query["graphId"].ToString();
            return await graphRepository.Find(id);
        }
    }
}
