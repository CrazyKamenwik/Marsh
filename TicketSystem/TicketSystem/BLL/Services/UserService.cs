using TicketSystem.DAL.Repositories.Abstractions;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserEntity> AddUserAsync(UserEntity user, CancellationToken cancellationToken)
        {
            return await _userRepository.CreateAsync(user, cancellationToken);
        }

        public async Task<IEnumerable<UserEntity>> GetUsersAsync(CancellationToken cancellationToken)
        {
            return await _userRepository.GetAllAsync(cancellationToken);
        }

        public async Task<UserEntity?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userByCondition =
                await _userRepository.GetUsersByConditionsAsync(cancellationToken, user => user.Id == id);

            return userByCondition.FirstOrDefault();
        }

        public async Task<UserEntity?> UpdateUserAsync(UserEntity user, CancellationToken cancellationToken)
        {
            var usersByCondition =
                await _userRepository.GetUsersByConditionsAsync(cancellationToken, u => u.Id == user.Id);
            if (!usersByCondition.Any())
                return null;

            return await _userRepository.UpdateAsync(user, cancellationToken);
        }

        public async Task<UserEntity?> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            return await _userRepository.DeleteAsync(id, cancellationToken);
        }

        public async Task<UserEntity?> GetNotBusyOperator(CancellationToken cancellationToken)
        {
            var operators = await _userRepository.GetUsersByConditionsAsync(cancellationToken, includeProperties: "Tickets",
                orderBy: u => u.OrderBy(users => users.Tickets));
            return operators.FirstOrDefault();
        }

    }
}