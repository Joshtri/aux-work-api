using AuxWork.Api.Models;
using AuxWork.Api.Repositories.Common;

namespace AuxWork.Api.Repositories.Projects;

public interface IProjectRepository
{
    Task<Project?> GetAsync(Guid id, CancellationToken ct = default);
    Task<List<Project>> ListAsync(bool includeArchived = false, CancellationToken ct = default);

    // NEW: search + paging
    Task<PagedResult<Project>> SearchAsync(bool includeArchived, string? q, int page, int pageSize, CancellationToken ct = default);

    Task<Project> AddAsync(Project p, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
}
