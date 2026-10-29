using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResearchAgent.DTOs;
using ResearchAgent.Services;

namespace ResearchAgent.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResearchController : ControllerBase
{
    private readonly ResearchService _research;
    public ResearchController(ResearchService research) => _research = research;
    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _research.GetAllAsync(UserId, page, pageSize));

    [HttpPost]
    public async Task<IActionResult> Start(StartResearchDto dto)
    {
        try { return Ok(await _research.StartResearchAsync(UserId, dto)); }
        catch (Exception ex) { return BadRequest(ex.Message); }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var session = await _research.GetSessionAsync(UserId, id);
        return session == null ? NotFound() : Ok(session);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _research.DeleteAsync(UserId, id);
        return deleted ? NoContent() : NotFound();
    }
}
