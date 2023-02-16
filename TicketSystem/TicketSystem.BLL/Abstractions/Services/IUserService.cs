using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Abstractions.Services;

public interface IUserService
{
    Task<User> AddUser(User userModel, CancellationToken cancellationToken);
    Task<User> GetUserById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsers(CancellationToken cancellationToken);
    Task<User> UpdateUser(User userModel, CancellationToken cancellationToken);
    Task DeleteUser(int id, CancellationToken cancellationToken);
    Task<User?> GetAvailableOperator(CancellationToken cancellationToken);
}