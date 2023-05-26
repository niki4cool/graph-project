using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GraphEditor.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using GraphEditor.Model.GraphModel;
using GraphEditor.Model;
using GraphEditor.Auth;
using GraphEditor.CRUD;
using GraphEditor.Model.Serialization;

namespace GraphEditor.Controllers.GraphController;

[ApiController]
[Route("/api/v1/[controller]")]
public class GraphController : Controller
{
    private readonly IAuthorizationService authorizationService;
    private readonly IRepository<Graph> graphRepository;
    private readonly IHubContext<GraphHub> graphHub;
    private readonly UserStore userStore;

    public GraphController(IAuthorizationService authorizationService,
                           IRepository<Graph> graphRepository,
                           IHubContext<GraphHub> graphHub,
                           IUserStore<User> userStore)
    {
        this.authorizationService = authorizationService;
        this.graphRepository = graphRepository;
        this.graphHub = graphHub;
        this.userStore = userStore as UserStore;
    }

    [HttpPut("id/{graphId}")]
    [Authorize]
    public async Task<ActionResult> Create([FromRoute] string graphId, [FromBody] CreateGraphRequest creationRequest)
    {
        var graphType = Enum.Parse<GraphType>(creationRequest.GraphType);
        var user = await userStore.FindByNameAsync(User.Identity!.Name!, CancellationToken.None);
        var graph = await graphRepository.Find(graphId);
        if (graph != null)
            return NoContent();
        if (graphType != GraphType.InstanceGraph)
            graph = new Graph(graphId, user!, graphType, null);

        if (graphType == GraphType.InstanceGraph)
        {
            if (creationRequest.GraphClassId == null)
                throw new ArgumentException(nameof(creationRequest.GraphClassId));
            var classGraph = await graphRepository.Find(creationRequest.GraphClassId);
            if (classGraph == null)
                throw new Exception("graph class must be specified for Regular graphs");
            graph = new Graph(graphId, user!, graphType, classGraph);
        }

        await graphRepository.Add(graph);

        user!.Creations.Add(graph);

        await userStore.UpdateAsync(user,
            CancellationToken.None);

        var path = ControllerContext.HttpContext.Request.Path;
        return Created(path, GraphSerializer.GraphToJson(graph));
    }

    [HttpGet("id/{graphId}")]
    [Authorize]
    public async Task<ActionResult> Get(string graphId)
    {
        var graph = await graphRepository.Find(graphId);
        if (graph == null)
            return NotFound();
        var authorizationResult = await authorizationService
            .AuthorizeAsync(User, graph, Operations.Read);
        if (!authorizationResult.Succeeded)
            return Unauthorized();

        return Ok(GraphSerializer.GraphToJson(graph));
    }

    [HttpDelete("id/{graphId}")]
    [Authorize]
    public async Task<ActionResult> Delete(string graphId)
    {
        var graph = await graphRepository.Find(graphId);
        if (graph == null)
            return NotFound();

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User, graph, Operations.Delete);
        if (!authorizationResult.Succeeded)
            return Unauthorized();

        await graphHub.Clients.Group(graphId).SendCoreAsync("OnGraphDelete", Array.Empty<object?>());
        await graphRepository.Delete(graphId);
        return Ok();
    }

    [HttpGet("list")]
    [Authorize]
    public async Task<ActionResult<GraphListResponse>> List()
    {
        var user = await userStore.FindByNameAsync(User!.Identity!.Name!,
                                            CancellationToken.None);
        if (user == null)
            return Unauthorized();
        var graphs = user.Creations
                .Concat(user.CanEdit)
                .Concat(user.CanView)
                .Distinct()
                .Select(g => GraphSerializer.GraphToJson(g))
                .ToList();

        return Ok(new GraphListResponse() { Graphs = graphs });
    }
}