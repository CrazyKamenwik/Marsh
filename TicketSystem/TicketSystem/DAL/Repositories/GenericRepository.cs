using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Repositories.Abstractions;

namespace TicketSystem.DAL.Repositories;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(DbContext context)
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

    public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        return await Include(includeProperties).ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _dbSet.FindAsync(new object?[] { id, cancellationToken }, cancellationToken: cancellationToken);
    }

    public async Task<TEntity?> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var query = Include(includeProperties);
        return await _dbSet.FindAsync(id, cancellationToken);
    }


   public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
       params Expression<Func<TEntity, object>>[] includeProperties)
   {
       var query = Include(includeProperties);
       return query.AsEnumerable().Where(predicate).ToList();
   }

    private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _dbSet.AsNoTracking();
        return includeProperties
            .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }



    //public async Task<IEnumerable<TEntity>> GetEntityByConditions(Expression<Func<TEntity, bool>>? filter = null,
    //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
    //    string includeProperties = "")
    //{
    //    IQueryable<TEntity> query = _context.Set<TEntity>();

    //    if (filter != null) query = query.Where(filter);

    //    foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
    //        query = query.Include(includeProperty);

    //    if (orderBy != null)
    //        return await orderBy(query).ToListAsync();

    //    return await query.ToListAsync();
    //}
}