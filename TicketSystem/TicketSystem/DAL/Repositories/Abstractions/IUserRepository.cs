using System.Linq.Expressions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.DAL.Repositories.Abstractions;

public interface IUserRepository
{
    Task<UserEntity> CreateAsync(UserEntity user, CancellationToken cancellationToken);
    Task<UserEntity> UpdateAsync(UserEntity user, CancellationToken cancellationToken);
    Task<UserEntity> DeleteAsync(UserEntity user, CancellationToken cancellationToken);

    Task<IEnumerable<UserEntity>> GetUsersByConditionsAsync(CancellationToken cancellationToken,
        Expression<Func<UserEntity, bool>>? filter = null,
        Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>? orderBy = null,
        string includeProperties = "");
}