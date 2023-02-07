using System.Linq.Expressions;
using Microsoft.AspNetCore.Routing.Matching;
using TicketSystem.Data.Models;

namespace TicketSystem.Data.Repositories.Abstractions
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user, CancellationToken cancellationToken);
        Task<User?> UpdateAsync(User user, CancellationToken cancellationToken);
        Task<User?> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetUsersByConditionsAsync(CancellationToken cancellationToken,
            Expression<Func<User, bool>>? filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>>? orderBy = null,
            string includeProperties = "");
        //Task<IEnumerable<User>> GetUsersByConditionsAsync(Expression<Func<User, bool>> conditions, CancellationToken cancellationToken);
    }
}
