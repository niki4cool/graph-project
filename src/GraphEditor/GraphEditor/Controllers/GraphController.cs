using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GraphEditor.Hubs;
using Microsoft.AspNetCore.Authorization;
using GraphEditor.Models.Graph;
using GraphEditor.Models.CRUD;
using System.Security.Claims;
using GraphEditor.Models.Auth.User;
using Microsoft.AspNetCore.Identity;
using GraphEditor.Models.Auth;

namespace GraphEditor.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class GraphController : Controller
{
    private readonly IAuthorizationService authorizationService;
    private readonly IRepository<GraphRecord> graphRepository;
    private readonly IHubContext<GraphHub> hubContext;
    private readonly IUserStore<UserRecord> userStore;

    private const string UserIsNull = "User == null";
    private const string HasNoId = "User[NameIdentifier] == null";
    private const string UserRecordIsNull = "UserRecord == null";

    public GraphController(IAuthorizationService auth,
                           IRepository<GraphRecord> repo,
                           IHubContext<GraphHub> hub,
                            IUserStore<UserRecord> userStore)
    {
        authorizationService = auth;
        graphRepository = repo;
        hubContext = hub;
        this.userStore = userStore;
    }

    [HttpPut("{graphId}")]
    public async Task<ActionResult> Create(string graphId)
    {        
        if (User == null)
            return Unauthorized(UserIsNull);

        var userId = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userId == null)
            return Unauthorized(HasNoId);

        var graph = await graphRepository.Find(graphId);
        if (graph != null)
            return NoContent();

        var userRecord = await userStore.FindByIdAsync(userId.Value,
            CancellationToken.None);
        if (userRecord == null)
            return Unauthorized(UserRecordIsNull);

        graph = new GraphRecord(graphId);

        userRecord.Creations.Add(graph);

        await userStore.UpdateAsync(userRecord,
            CancellationToken.None);

        var path = ControllerContext.HttpContext.Request.Path;
        return Created(path, ""); //TODO: return graph
    }

    [HttpGet("{graphId}")]
    public async Task<ActionResult> Get(string graphId)
    {
        if (User == null)
            return Unauthorized(UserIsNull);

        var graph = await graphRepository.Find(graphId);
        if (graph == null)
            return NotFound();
        var authorizationResult = await authorizationService
            .AuthorizeAsync(User, graph, Operations.Read);
        if (!authorizationResult.Succeeded)
            return Unauthorized();

        return Ok("");//TODO: return graph
    }

    [HttpDelete("{graphId}")]
    public async Task<ActionResult> Delete(string graphId)
    {
        if (User == null)
            return Unauthorized(UserIsNull);

        var graph = await graphRepository.Find(graphId);
        if (graph == null)
            return NotFound();

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User, graph, Operations.Delete);
        if (!authorizationResult.Succeeded)
            return Unauthorized();

        await hubContext.Clients.Group(graphId).SendCoreAsync("OnGraphDelete", Array.Empty<object?>());
        await graphRepository.Delete(graphId);
        return Ok();
    }
}