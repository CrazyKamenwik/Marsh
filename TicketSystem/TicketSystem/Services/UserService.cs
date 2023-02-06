using Microsoft.EntityFrameworkCore;
using TicketSystem.Data.Models;
using TicketSystem.Data.Repositories;

namespace TicketSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUserAsync(User user)
        {
            return await _userRepository.CreateAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUsersByConditionsAsync(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            if (id != user.Id)
                return null;

            return await _userRepository.UpdateAsync(id, user);
        }

        public async Task<User?> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<User?> GetFreeOperator()
        {
            return await _userRepository.GetUsersByConditionsAsync(u => u.UserRole == UserRole.Operator)
                .OrderBy(u => u.Tickets)
                .FirstOrDefaultAsync();
        }
    }
}
