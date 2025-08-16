// Controllers/ProjectsController.cs
using AuxWork.Api.Common;
using AuxWork.Api.DTOs;
using AuxWork.Api.Services;                 // IProjectsService
using Microsoft.AspNetCore.Mvc;

namespace AuxWork.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectsService _svc;
    private readonly ILogger<ProjectsController> _log;

    public ProjectsController(IProjectsService svc, ILogger<ProjectsController> log)
    {
        _svc = svc;
        _log = log;
    }

    // GET /api/projects?includeArchived=false&q=&page=1&pageSize=20
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ProjectDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> List(
        [FromQuery] bool includeArchived = false,
        [FromQuery] string? q = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken ct = default)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var result = await _svc.SearchAsync(includeArchived, q, page, pageSize, ct);
        var resp = PagedResponses.Success(result.Items, result.Page, result.PageSize, result.Total);
        return Ok(resp);
    }

    // contoh untuk Get by id:
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<ProjectDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(Guid id, CancellationToken ct = default)
    {
        var dto = await _svc.GetAsync(id, ct);
        return dto is null ? NotFound(ApiResponses.Fail<ProjectDto>("Not found"))
                           : Ok(ApiResponses.Success(dto));
    }

    // POST /api/projects
    [HttpPost]
    [ProducesResponseType(typeof(ProjectDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateProjectDto dto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var created = await _svc.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }
        catch (InvalidOperationException ex) // mis. code duplikat
        {
            _log.LogWarning(ex, "Create project conflict");
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT /api/projects/{id}
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProjectDto dto, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var updated = await _svc.UpdateAsync(id, dto, ct);
            return updated is null ? NotFound() : NoContent();
        }
        catch (InvalidOperationException ex) // mis. code duplikat
        {
            _log.LogWarning(ex, "Update project conflict");
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE /api/projects/{id}
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct = default)
    {
        var ok = await _svc.DeleteAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    // POST /api/projects/{id}/archive
    [HttpPost("{id:guid}/archive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Archive(Guid id, CancellationToken ct = default)
    {
        var ok = await _svc.ArchiveAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }

    // POST /api/projects/{id}/unarchive
    [HttpPost("{id:guid}/unarchive")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Unarchive(Guid id, CancellationToken ct = default)
    {
        var ok = await _svc.UnarchiveAsync(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
