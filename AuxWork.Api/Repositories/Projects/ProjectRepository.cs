using AuxWork.Api.Data;
using AuxWork.Api.Models;
using AuxWork.Api.Repositories.Common;
using Microsoft.EntityFrameworkCore;

namespace AuxWork.Api.Repositories.Projects;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _db;
    public ProjectRepository(ApplicationDbContext db) => _db = db;

    public Task<Project?> GetAsync(Guid id, CancellationToken ct = default)
        => _db.Projects.FirstOrDefaultAsync(p => p.Id == id, ct); // tracked

    public Task<List<Project>> ListAsync(bool includeArchived = false, CancellationToken ct = default)
        => _db.Projects.Where(p => includeArchived || !p.IsArchived)
                       .OrderBy(p => p.Name)
                       .AsNoTracking()
                       .ToListAsync(ct);

    public async Task<PagedResult<Project>> SearchAsync(bool includeArchived, string? q, int page, int pageSize, CancellationToken ct = default)
    {
        var query = _db.Projects.AsQueryable();
        if (!includeArchived) query = query.Where(p => !p.IsArchived);
        if (!string.IsNullOrWhiteSpace(q))
        {
            var like = $"%{q.Trim()}%";
            query = query.Where(p => EF.Functions.ILike(p.Name, like) || EF.Functions.ILike(p.Code ?? "", like));
        }

        var total = await query.CountAsync(ct);
        var items = await query.OrderBy(p => p.Name)
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize)
                               .AsNoTracking()
                               .ToListAsync(ct);

        return new(items, page, pageSize, total);
    }

    public async Task<Project> AddAsync(Project p, CancellationToken ct = default)
    { _db.Projects.Add(p); await _db.SaveChangesAsync(ct); return p; }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var p = await _db.Projects.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (p is null) return false;
        _db.Projects.Remove(p);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
