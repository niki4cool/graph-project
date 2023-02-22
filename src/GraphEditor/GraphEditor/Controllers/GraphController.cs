using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GraphEditor.Hubs;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata;
using GraphEditor.Models.Graph;
using GraphEditor.Models.CRUD;

namespace GraphEditor.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class GraphController : Controller
{
    private readonly IAuthorizationService authorizationService;
    private readonly IRepository<GraphRecord> graphRepository;
    private readonly IHubContext<GraphHub> hubContext;

    public GraphController(IAuthorizationService auth,
                           IRepository<GraphRecord> repo,
                           IHubContext<GraphHub> hub)
    {
        authorizationService = auth;
        graphRepository = repo;
        hubContext = hub;
    }

    [HttpPut("{graphId}")]
    public async Task<ActionResult> Create(string graphId)
    {
        if (User == null)
            Unauthorized();
        var graph = await graphRepository.Find(graphId);
        if (graph != null)
            return NoContent();
        graph = new GraphRecord(graphId);        
        var path = ControllerContext.HttpContext.Request.Path;        
        await graphRepository.Add(graph);
        return Created(path, JsonSerializer.Serialize(graph));
    }

    [HttpGet("{graphId}")]
    public async Task<ActionResult> Get(string graphId)
    {
        var graph = await graphRepository.Find(graphId);
        if (graph == null)
            return NotFound();

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User, graph, StringConstants.GraphCRUDPolicy);
        if (!authorizationResult.Succeeded)
            return Unauthorized();

        return Ok(graphId);
    }

    [HttpDelete("{graphId}")]
    public async Task<ActionResult> Delete(string graphId)
    {
        var graph = await graphRepository.Find(graphId);
        if (graph == null)
            return NotFound();

        var authorizationResult = await authorizationService
            .AuthorizeAsync(User, graph, StringConstants.GraphCRUDPolicy);
        if (!authorizationResult.Succeeded)
            return Unauthorized();

        await hubContext.Clients.Group(graphId).SendCoreAsync("OnGraphDelete", Array.Empty<object?>());
        await graphRepository.Delete(graphId);
        return Ok();
    }
}