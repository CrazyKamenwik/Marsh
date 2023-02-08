using TicketSystem.DAL.Entities;

namespace TicketSystem.BLL.Services.Abstractions
{
    public interface IUserService
    {
        Task<UserEntity> AddUserAsync(UserEntity user, CancellationToken cancellationToken);
        Task<UserEntity?> GetUserByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<UserEntity>> GetUsersAsync(CancellationToken cancellationToken);
        Task<UserEntity?> UpdateUserAsync(UserEntity user, CancellationToken cancellationToken);
        Task<UserEntity?> DeleteUserAsync(int id, CancellationToken cancellationToken);
        Task<UserEntity?> GetNotBusyOperator(CancellationToken cancellationToken);
    }
}
