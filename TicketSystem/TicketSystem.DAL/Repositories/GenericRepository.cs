using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    private readonly ApplicationContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ApplicationContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    public async Task Create(TEntity item, CancellationToken cancellationToken)
    {
        _dbSet.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Update(CancellationToken cancellationToken, params object[] items)
    {
        _context.UpdateRange(items);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Remove(int id, CancellationToken cancellationToken)
    {
        var item = await _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (item == null)
            return;

        _dbSet.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TEntity>> GetWithInclude(CancellationToken cancellationToken,
        Func<TEntity, bool>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Include(includeProperties);

        if (orderBy != null) query = orderBy.Invoke(query);

        return predicate != null ? query.AsEnumerable().Where(predicate) : await query.ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdWithInclude(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> entity = _dbSet;

        if (includeProperties.Length > 0)
            entity = Include(includeProperties);

        return await entity.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }
}