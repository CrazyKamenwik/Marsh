﻿using System.Linq.Expressions;
using TicketSystem.DAL.Entities.Abstractions;

namespace TicketSystem.DAL.Repositories.Abstractions;

public interface IGenericRepository<TEntity> where TEntity : IBaseEntity
{
    public Task CreateAsync(TEntity item, CancellationToken cancellationToken);
    public Task UpdateAsync(TEntity item, CancellationToken cancellationToken);
    public Task RemoveAsync(TEntity item, CancellationToken cancellationToken);

    public Task<IEnumerable<TEntity>> GetAsync();

    public Task<IEnumerable<TEntity>> GetWithIncludeAsync(CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public IEnumerable<TEntity> GetWithInclude(Func<TEntity, bool> predicate,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken);

    public Task<TEntity?> GetByIdWithIncludeAsync(int id, CancellationToken cancellationToken,
        params Expression<Func<TEntity, object>>[] includeProperties);

    public Task SaveAsync();
}