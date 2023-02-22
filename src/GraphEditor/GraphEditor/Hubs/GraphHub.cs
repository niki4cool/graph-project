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

            var authorizationResult = await authorizationService
                .AuthorizeAsync(Context.User, graph, StringConstants.GraphCRUDPolicy);
            if (!authorizationResult.Succeeded)
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

        public async Task UpdateGraph(GraphRecord newGraph)
        {
            var graph = await GetContextGraph();
            var isValid = graph != null
                       && newGraph.Id != graph.Id 
                       && !await CanEdit(graph);

            if (!isValid)
            {
                Context.Abort();
                return;
            }

            await graphRepository.Update(newGraph);
            await Clients.GroupExcept(graph.Id, Context.ConnectionId)
                  .SendCoreAsync("OnGraphUpdate", new object[] { newGraph });
        }

        public async Task RequestUpdate()
        {
            var graph = await GetContextGraph();
            if (graph == null)
            {
                Context.Abort();
                return;
            }


            if (!await CanView(graph))
            {
                Context.Abort();
                return;
            }

            await Clients.Caller
                          .SendCoreAsync("OnGraphUpdate", new object[] { graph });
        }

        private async Task<bool> CanEdit(GraphRecord graph)
        {
            var authorizationResult = await authorizationService
                .AuthorizeAsync(Context.User, graph, StringConstants.GraphCRUDPolicy);
            return authorizationResult.Succeeded;
        }

        private async Task<bool> CanView(GraphRecord graph)
        {
            var authorizationResult = await authorizationService
                .AuthorizeAsync(Context.User, graph, StringConstants.GraphCRUDPolicy);
            return authorizationResult.Succeeded;
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
