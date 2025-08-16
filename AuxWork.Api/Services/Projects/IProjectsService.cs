using AuxWork.Api.DTOs;
using AuxWork.Api.Repositories.Common;

namespace AuxWork.Api.Services;

public interface IProjectsService
{
    Task<ProjectDto> CreateAsync(CreateProjectDto dto, CancellationToken ct = default);
    Task<List<ProjectDto>> ListAsync(bool includeArchived, CancellationToken ct = default);
    Task<ProjectDto?> GetAsync(Guid id, CancellationToken ct = default);
    Task<ProjectDto?> UpdateAsync(Guid id, UpdateProjectDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<bool> ArchiveAsync(Guid id, CancellationToken ct = default);
    Task<bool> UnarchiveAsync(Guid id, CancellationToken ct = default);

    // NEW
    Task<PagedResult<ProjectDto>> SearchAsync(bool includeArchived, string? q, int page, int pageSize, CancellationToken ct = default);
}
