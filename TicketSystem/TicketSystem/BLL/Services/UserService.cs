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
        var allUsers =
            await _userRepository.GetWithIncludeAsync(cancellationToken, u => u.UserRole);

        return _mapper.Map<IEnumerable<UserModel>>(allUsers);
    }

    public async Task<UserModel> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var userEntityByCondition = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (userEntityByCondition == null)
            throw new NotFoundException($"User with id {id} not found");

        return _mapper.Map<UserModel>(userEntityByCondition);
    }

    public async Task<UserModel> UpdateUserAsync(UserModel user, CancellationToken cancellationToken)
    {
        var usersEntity = await _userRepository.GetByIdAsync(user.Id, cancellationToken);

        if (usersEntity == null)
            throw new NotFoundException($"User with id {user.Id} not found");

        var userEntity = _mapper.Map<UserEntity>(user);
        await _userRepository.UpdateAsync(userEntity, cancellationToken);

        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<UserModel> DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        var usersEntity = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (usersEntity == null)
            throw new NotFoundException($"User with id {id} not found");

        await _userRepository.RemoveAsync(usersEntity, cancellationToken);

        return _mapper.Map<UserModel>(usersEntity);
    }

    public async Task<UserModel?> GetAvailableOperator(CancellationToken cancellationToken)
    {
        var operatorsEntityByCondition = _userRepository.GetWithInclude(u => u.UserRole.Name == "Operator",
            u => u.OrderBy(users => users.Tickets!.Count),
            u => u.Tickets);

        var operatorEntity = operatorsEntityByCondition.FirstOrDefault();

        return operatorEntity == null ? null : _mapper.Map<UserModel>(operatorEntity);
    }
}