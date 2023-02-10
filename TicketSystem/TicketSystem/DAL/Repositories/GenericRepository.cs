using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Repositories.Abstractions;

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

    public async Task<IEnumerable<TEntity>> GetAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await Include(includeProperties).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(new object?[] { id, cancellationToken }, cancellationToken);
    }

    public async Task<TEntity?> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Include(includeProperties);
        return await query.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Include(includeProperties);

        orderBy?.Invoke(query).ToListAsync();

        return query.AsEnumerable().Where(predicate).ToList();
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