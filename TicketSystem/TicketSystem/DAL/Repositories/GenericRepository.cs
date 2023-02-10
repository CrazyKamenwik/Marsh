using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Repositories.Abstractions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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

    public async Task CreateAsync(TEntity item, CancellationToken cancellationToken)
    {
        _dbSet.Add(item);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(TEntity item, CancellationToken cancellationToken)
    {
        _context.Entry(item).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveAsync(TEntity item, CancellationToken cancellationToken)
    {
        _dbSet.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
    }

    //public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(CancellationToken cancellationToken,
    //    params Expression<Func<TEntity, object>>[] includeProperties)
    //{
    //    return await Include(includeProperties).ToListAsync(cancellationToken);
    //}
    public async Task<IEnumerable<TEntity>> GetWithInclude(CancellationToken cancellationToken, Func<TEntity, bool>? predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Include(includeProperties);

        if (orderBy != null)
        {
            await orderBy.Invoke(query).ToListAsync(cancellationToken);
        }

        var entities = query.AsEnumerable();

        if (predicate != null)
            entities = entities.Where(predicate);

        return entities.ToList();
    }

    public async Task<TEntity?> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> entity = _dbSet;

        if (includeProperties.Length > 0)
            entity = Include(includeProperties);

        return await entity.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }
}