// Services/Projects/ProjectsService.cs
using AuxWork.Api.DTOs;
using AuxWork.Api.Models;
using AuxWork.Api.Repositories.Abstractions;
using AuxWork.Api.Repositories.Common;
using AuxWork.Api.Repositories.Projects;

namespace AuxWork.Api.Services.Projects;

public class ProjectsService : IProjectsService
{
    private readonly IProjectRepository _repo;
    private readonly IUnitOfWork _uow;

    public ProjectsService(IProjectRepository repo, IUnitOfWork uow)
    {
        _repo = repo;
        _uow = uow;
    }

    public async Task<PagedResult<ProjectDto>> SearchAsync(bool includeArchived, string? q, int page, int pageSize, CancellationToken ct = default)
    {
        var pr = await _repo.SearchAsync(includeArchived, q, page, pageSize, ct);
        var mapped = pr.Items.Select(p => new ProjectDto(p.Id, p.Name, p.Code, p.IsArchived, p.CreatedAtUtc, p.UpdatedAtUtc)).ToList();
        return new(mapped, pr.Page, pr.PageSize, pr.Total);
    }

    public async Task<ProjectDto> CreateAsync(CreateProjectDto dto, CancellationToken ct = default)
    {
        var p = new Project
        {
            Id = Guid.NewGuid(),
            Name = dto.Name.Trim(),
            Code = dto.Code?.Trim(),
            IsArchived = false,
            CreatedAtUtc = DateTime.UtcNow,
            UpdatedAtUtc = DateTime.UtcNow
        };

        await _repo.AddAsync(p, ct);
        await _uow.SaveChangesAsync(ct);
        return Map(p);
    }

    public async Task<List<ProjectDto>> ListAsync(bool includeArchived, CancellationToken ct = default)
        => (await _repo.ListAsync(includeArchived, ct)).Select(Map).ToList();

    public async Task<ProjectDto?> GetAsync(Guid id, CancellationToken ct = default)
        => (await _repo.GetAsync(id, ct)) is { } p ? Map(p) : null;

    public async Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectDto dto, CancellationToken ct = default)
    {
        var p = await _repo.GetAsync(id, ct);       // <- pakai GetAsync, bukan GetByIdAsync
        if (p is null) return null;

        p.Name = dto.Name.Trim();
        p.Code = dto.Code?.Trim();
        p.IsArchived = dto.IsArchived;
        p.UpdatedAtUtc = DateTime.UtcNow;

        // kalau GetAsync kamu "AsNoTracking", lihat catatan di bawah
        await _uow.SaveChangesAsync(ct);
        return Map(p);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        => _repo.DeleteAsync(id, ct);

    public async Task<bool> ArchiveAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repo.GetAsync(id, ct);
        if (p is null) return false;
        p.IsArchived = true;
        p.UpdatedAtUtc = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> UnarchiveAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _repo.GetAsync(id, ct);
        if (p is null) return false;
        p.IsArchived = false;
        p.UpdatedAtUtc = DateTime.UtcNow;
        await _uow.SaveChangesAsync(ct);
        return true;
    }

    private static ProjectDto Map(Project p)
        => new(p.Id, p.Name, p.Code, p.IsArchived, p.CreatedAtUtc, p.UpdatedAtUtc);
}
