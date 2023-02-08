using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Services.Abstractions
{
    public interface IUserService
    {
        Task<UserModel> AddUserAsync(UserModel user, CancellationToken cancellationToken);
        Task<UserModel?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken);
        Task<UserModel?> UpdateUserAsync(UserModel user, CancellationToken cancellationToken);
        Task<UserModel?> DeleteUserAsync(int id, CancellationToken cancellationToken);
        Task<UserModel?> GetNotBusyOperator(CancellationToken cancellationToken);
    }
}
