using GraphEditor.Models;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using GraphEditor.Hubs;

namespace GraphEditor.Controllers;

[ApiController]
[Route("/api/v1/[controller]")]
public class GraphController : Controller
{
    private readonly IGraphRepository graphRepository;
    private readonly IHubContext<GraphHub> hubContext;

    public GraphController(IGraphRepository repo, IHubContext<GraphHub> hub)
    {
        graphRepository = repo;
        hubContext = hub;
    }

    [HttpGet("{graphId}")]
    public async Task<ActionResult> Get(string graphId)
    {
        var graph = await graphRepository.Get(graphId);
        if (graph == null)
            return NotFound();
        return Ok();
    }

    [HttpPut("{graphId}")]
    public async Task<ActionResult> Create(string graphId)
    {
        var graph = await graphRepository.Get(graphId);
        if (graph != null)
            return NoContent();
        graph = new GraphRecord();
        var path = ControllerContext.HttpContext.Request.Path;
        graph.Id = graphId;
        await graphRepository.Create(graph);
        return Created(path, JsonSerializer.Serialize(graph));
    }

    [HttpDelete("{graphId}")]
    public async Task<ActionResult> Delete(string graphId)
    {
        await hubContext.Clients.Group(graphId).SendCoreAsync("OnGraphDelete", Array.Empty<object?>());
        await graphRepository.Delete(graphId);
        return Ok();
    }
}