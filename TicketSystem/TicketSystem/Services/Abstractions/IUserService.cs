using TicketSystem.Data.Models;

namespace TicketSystem.Services.Abstractions
{
    public interface IUserService
    {
        Task<User> AddUserAsync(User user);
        Task<User?> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> UpdateUserAsync(User user);
        Task<User?> DeleteUserAsync(int id);
        Task<User?> GetFreeOperator();
    }
}
