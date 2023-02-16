using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Abstractions.Services;

public interface IUserService
{
    Task<User> AddUserAsync(User userModel, CancellationToken cancellationToken);
    Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
    Task<User> UpdateUserAsync(User userModel, CancellationToken cancellationToken);
    Task DeleteUserAsync(int id, CancellationToken cancellationToken);
    Task<User?> GetAvailableOperator(CancellationToken cancellationToken);
}