using AutoMapper;
using TicketSystem.BLL.Exceptions;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Repositories.Abstractions;

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

    public async Task<UserModel> AddUserAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        await _userRepository.CreateAsync(userEntity, cancellationToken);

        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var allUsers = await _userRepository.GetWithInclude(cancellationToken,
            isTrack: false,
            predicate: null,
            orderBy: null,
            includeProperties: u => u.UserRole);

        return _mapper.Map<IEnumerable<UserModel>>(allUsers);
    }

    public async Task<UserModel> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var userEntityByCondition =
            await _userRepository.GetByIdWithIncludeAsync(id, cancellationToken, u => u.UserRole, u => u.Tickets);

        if (userEntityByCondition == null)
            throw new NotFoundException($"User with id {id} not found");

        return _mapper.Map<UserModel>(userEntityByCondition);
    }

    public async Task<UserModel> UpdateUserAsync(UserModel user, CancellationToken cancellationToken)
    {
        var usersEntity = await _userRepository.GetByIdWithIncludeAsync(user.Id, cancellationToken);

        if (usersEntity == null)
            throw new NotFoundException($"User with id {user.Id} not found");

        var userEntity = _mapper.Map<UserEntity>(user);
        await _userRepository.UpdateAsync(userEntity, cancellationToken);

        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<UserModel> DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        var usersEntity = await _userRepository.GetByIdWithIncludeAsync(id, cancellationToken);

        if (usersEntity == null)
            throw new NotFoundException($"User with id {id} not found");

        await _userRepository.RemoveAsync(usersEntity, cancellationToken);

        return _mapper.Map<UserModel>(usersEntity);
    }

    public async Task<UserModel?> GetAvailableOperator(CancellationToken cancellationToken)
    {
        var availableOperators = await _userRepository.GetWithInclude(cancellationToken,
            false,
            u => u.UserRole.Name == "Operator",
            q => q.OrderBy(u => u.Tickets.Count()),
            u => u.UserRole,
            u => u.Tickets);

        var availableOperator = availableOperators.FirstOrDefault();

        return availableOperator == null ? null : _mapper.Map<UserModel>(availableOperator);
    }
}