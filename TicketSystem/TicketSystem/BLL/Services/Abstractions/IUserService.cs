using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Services.Abstractions;

public interface IUserService
{
    Task<UserModel> AddUserAsync(UserModel userModel, CancellationToken cancellationToken);
    Task<UserModel> GetUserByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken);
    Task<UserModel> UpdateUserAsync(UserModel userModel, CancellationToken cancellationToken);
    Task<UserModel> DeleteUserAsync(int id, CancellationToken cancellationToken);
    Task<UserModel?> GetAvailableOperator(CancellationToken cancellationToken);
}