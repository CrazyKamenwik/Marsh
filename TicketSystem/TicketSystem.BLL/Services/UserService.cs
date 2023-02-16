using AutoMapper;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.BLL.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<UserEntity> _userRepository;

    public UserService(IGenericRepository<UserEntity> userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<User> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        await _userRepository.CreateAsync(userEntity, cancellationToken);

        return _mapper.Map<User>(userEntity);
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var allUsers = await _userRepository.GetWithInclude(cancellationToken,
            false,
            null,
            null,
            u => u.UserRole);

        return _mapper.Map<IEnumerable<User>>(allUsers);
    }

    public async Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var userEntityByCondition =
            await _userRepository.GetByIdWithIncludeAsync(id, cancellationToken, u => u.UserRole, u => u.Tickets);

        return _mapper.Map<User>(userEntityByCondition);
    }

    public async Task<User> UpdateUserAsync(User user, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<UserEntity>(user);

        await _userRepository.UpdateAsync(userEntity, cancellationToken);

        return _mapper.Map<User>(userEntity);
    }

    public async Task DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        await _userRepository.RemoveAsync(id, cancellationToken);
    }

    public async Task<User?> GetAvailableOperator(CancellationToken cancellationToken)
    {
        var availableOperators = await _userRepository.GetWithInclude(cancellationToken,
            false,
            u => u.UserRole.Name == "Operator",
            q => q.OrderBy(u => u.Tickets.Count()),
            u => u.UserRole,
            u => u.Tickets);

        var availableOperator = availableOperators.FirstOrDefault();

        return availableOperator == null ? null : _mapper.Map<User>(availableOperator);
    }
}