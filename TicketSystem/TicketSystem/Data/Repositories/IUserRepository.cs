using System.Linq.Expressions;
using TicketSystem.Data.Models;

namespace TicketSystem.Data.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User?> UpdateAsync(int id, User user);
        Task<User?> DeleteAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        IQueryable<User> GetUsersByConditionsAsync(Expression<Func<User, bool>> conditions);
    }
}
