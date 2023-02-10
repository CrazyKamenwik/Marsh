using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace TicketSystem.DAL.Repositories.Abstractions;

public interface IGenericRepository<TEntity>
{
    public Task CreateAsync(TEntity item, CancellationToken cancellationToken);
    public Task UpdateAsync(TEntity item, CancellationToken cancellationToken);
    public Task RemoveAsync(TEntity item, CancellationToken cancellationToken);

    public Task<IEnumerable<TEntity>> GetAsync();
    public Task<IEnumerable<TEntity>> GetWithIncludeAsync(CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties);
    public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);
    public Task<TEntity?> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public Task SaveAsync();
}