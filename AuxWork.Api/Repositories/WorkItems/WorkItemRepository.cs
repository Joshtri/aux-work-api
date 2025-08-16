using AuxWork.Api.Data;
using AuxWork.Api.Models;
using AuxWork.Api.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace AuxWork.Api.Repositories.WorkItems;

public class WorkItemRepository : IWorkItemRepository
{
    private readonly ApplicationDbContext _db;
    public WorkItemRepository(ApplicationDbContext db) => _db = db;

    public Task<WorkItem?> GetAsync(Guid id, CancellationToken ct = default)
        => _db.WorkItems
              .Include(x => x.WorkItemLabels).ThenInclude(wl => wl.Label)
              .Include(x => x.Checklist)
              .AsNoTracking()
              .FirstOrDefaultAsync(x => x.Id == id, ct);

    public async Task<WorkItem> AddAsync(WorkItem item, CancellationToken ct = default)
    {
        _db.WorkItems.Add(item);
        await _db.SaveChangesAsync(ct);
        return item;
    }

    public async Task<bool> UpdateAsync(WorkItem item, CancellationToken ct = default)
    {
        var exists = await _db.WorkItems.AnyAsync(x => x.Id == item.Id, ct);
        if (!exists) return false;
        item.UpdatedAtUtc = DateTime.UtcNow;
        _db.WorkItems.Update(item);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var wi = await _db.WorkItems.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (wi is null) return false;
        _db.WorkItems.Remove(wi);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<PagedResult<WorkItem>> SearchAsync(WorkItemQuery q, CancellationToken ct = default)
    {
        var query = _db.WorkItems
            .Include(x => x.WorkItemLabels).ThenInclude(wl => wl.Label)
            .AsQueryable();

        if (q.ProjectId is Guid pid) query = query.Where(x => x.ProjectId == pid);
        if (q.Type is { } t) query = query.Where(x => x.Type == t);
        if (q.Status is { } s) query = query.Where(x => x.Status == s);
        if (q.AssigneeId is Guid a) query = query.Where(x => x.AssigneeId == a);
        if (!string.IsNullOrWhiteSpace(q.Label))
            query = query.Where(x => x.WorkItemLabels.Any(l => l.Label!.Name == q.Label));
        if (!string.IsNullOrWhiteSpace(q.Q))
        {
            var like = $"%{q.Q.Trim()}%";
            // ILIKE (PostgreSQL) via EF.Functions
            query = query.Where(x =>
                EF.Functions.ILike(x.Title, like) ||
                EF.Functions.ILike(x.Description ?? "", like));
        }
        if (q.DueFrom is { } df) query = query.Where(x => x.DueDate >= df.Date);
        if (q.DueTo is { } dt) query = query.Where(x => x.DueDate <= dt.Date);

        // sorting: "-priority,createdAt"
        IOrderedQueryable<WorkItem>? ordered = null;
        foreach (var token in (q.Sort ?? "-priority,createdAt").Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            var desc = token.StartsWith("-");
            var col = desc ? token[1..] : token;
            ordered = (col.ToLower()) switch
            {
                "priority" => (ordered == null ? (desc ? query.OrderByDescending(x => x.Priority) : query.OrderBy(x => x.Priority))
                                                 : (desc ? ordered.ThenByDescending(x => x.Priority) : ordered.ThenBy(x => x.Priority))),
                "duedate" => (ordered == null ? (desc ? query.OrderByDescending(x => x.DueDate) : query.OrderBy(x => x.DueDate))
                                                 : (desc ? ordered.ThenByDescending(x => x.DueDate) : ordered.ThenBy(x => x.DueDate))),
                "createdat" => (ordered == null ? (desc ? query.OrderByDescending(x => x.CreatedAtUtc) : query.OrderBy(x => x.CreatedAtUtc))
                                                 : (desc ? ordered.ThenByDescending(x => x.CreatedAtUtc) : ordered.ThenBy(x => x.CreatedAtUtc))),
                _ => ordered ?? query.OrderByDescending(x => x.Priority)
            };
            query = ordered;
        }

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((q.Page - 1) * q.PageSize)
            .Take(q.PageSize)
            .AsNoTracking()
            .ToListAsync(ct);

        return new PagedResult<WorkItem>(items, q.Page, q.PageSize, total);
    }
}
