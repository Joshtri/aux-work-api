using AuxWork.Api.Models;
using AuxWork.Api.Repositories.Common;

namespace AuxWork.Api.Repositories.WorkItems;

public interface IWorkItemRepository
{
    Task<WorkItem?> GetAsync(Guid id, CancellationToken ct = default);
    Task<WorkItem> AddAsync(WorkItem item, CancellationToken ct = default);
    Task<bool> UpdateAsync(WorkItem item, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<PagedResult<WorkItem>> SearchAsync(WorkItemQuery q, CancellationToken ct = default);
}
