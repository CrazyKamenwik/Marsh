using System.Linq.Expressions;

namespace TicketSystem.DAL.Abstractions;

public interface IGenericRepository<TEntity> where TEntity : IBaseEntity
{
    public Task Create(TEntity item, CancellationToken cancellationToken);
    public Task Update(TEntity item, CancellationToken cancellationToken);
    public Task Remove(int id, CancellationToken cancellationToken);

    public Task<TEntity?> GetByIdWithInclude(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public Task<IEnumerable<TEntity>> GetWithInclude(CancellationToken cancellationToken,
        bool isTrack,
        Func<TEntity, bool>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public Task Save();
}