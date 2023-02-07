using TicketSystem.Data.Models;

namespace TicketSystem.Services.Abstractions
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user, CancellationToken cancellationToken);
        Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
        Task<User?> UpdateUserAsync(User user, CancellationToken cancellationToken);
        Task<User?> DeleteUserAsync(int id, CancellationToken cancellationToken);
        Task<User?> GetNotBusyOperator(CancellationToken cancellationToken);
    }
}
