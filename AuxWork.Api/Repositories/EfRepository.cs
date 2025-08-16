using System.Linq.Expressions;
using AuxWork.Api.Data;
using AuxWork.Api.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AuxWork.Api.Repositories;

public class EfRepository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<T> _set;

    public EfRepository(ApplicationDbContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
        => await _set.FindAsync([id], ct);

    public async Task<List<T>> ListAsync(Expression<Func<T, bool>>? predicate = null,
                                         CancellationToken ct = default,
                                         params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> q = _set.AsQueryable();
        if (predicate is not null) q = q.Where(predicate);
        if (includes?.Length > 0)
            q = includes.Aggregate(q, (acc, inc) => acc.Include(inc));
        return await q.AsNoTracking().ToListAsync(ct);
    }

    public Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken ct = default)
        => predicate is null ? _set.CountAsync(ct) : _set.CountAsync(predicate, ct);

    public Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => _set.AnyAsync(predicate, ct);

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _set.AddAsync(entity, ct);
        return entity;
    }

    public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken ct = default)
        => _set.AddRangeAsync(entities, ct);

    public void Update(T entity) => _set.Update(entity);

    public void Remove(T entity) => _set.Remove(entity);
}
