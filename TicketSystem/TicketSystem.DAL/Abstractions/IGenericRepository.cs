using System.Linq.Expressions;

namespace TicketSystem.DAL.Abstractions;

public interface IGenericRepository<TEntity> where TEntity : IBaseEntity
{
    public Task CreateAsync(TEntity item, CancellationToken cancellationToken);
    public Task UpdateAsync(TEntity item, CancellationToken cancellationToken);
    public Task RemoveAsync(int id, CancellationToken cancellationToken);

    public Task<TEntity> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public Task<IEnumerable<TEntity>> GetWithInclude(CancellationToken cancellationToken,
        bool isTrack,
        Func<TEntity, bool>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public Task SaveAsync();
}