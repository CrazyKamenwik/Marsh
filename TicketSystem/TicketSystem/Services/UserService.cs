using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data.Models;
using TicketSystem.Data.Models.Enums;
using TicketSystem.Data.Repositories.Abstractions;
using TicketSystem.Services.Abstractions;

namespace TicketSystem.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
        {
            return await _userRepository.CreateAsync(user, cancellationToken);
        }

        public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userByCondition =
                await _userRepository.GetUsersByConditionsAsync(cancellationToken, user => user.Id == id);

            return userByCondition.FirstOrDefault();
        }

        public async Task<User?> UpdateUserAsync(User user, CancellationToken cancellationToken)
        {
            return await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task<User?> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<User?> GetFreeOperator(CancellationToken cancellationToken)
        {
            var operators = await _userRepository.GetUsersByConditionsAsync(cancellationToken, includeProperties: "Tickets",
                orderBy: u => u.OrderBy(users => users.Tickets));
            return operators.FirstOrDefault();
        }

    }
}
