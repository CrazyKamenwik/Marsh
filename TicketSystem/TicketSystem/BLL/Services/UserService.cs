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
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<UserModel> AddUserAsync(UserModel user, CancellationToken cancellationToken)
    {
        var userEntity = _mapper.Map<UserEntity>(user);
        userEntity = await _userRepository.CreateAsync(userEntity, cancellationToken);

        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<IEnumerable<UserModel>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var allUsers =
            await _userRepository.GetUsersByConditionsAsync(cancellationToken, includeProperties: "UserRole");

        return _mapper.Map<IEnumerable<UserModel>>(allUsers);
    }

    public async Task<UserModel> GetUserByIdAsync(int id, CancellationToken cancellationToken)
    {
        var userEntityByCondition =
            await _userRepository.GetUsersByConditionsAsync(cancellationToken, user => user.Id == id);
        var userEntity = userEntityByCondition.FirstOrDefault();

        if (userEntity == null)
            throw new NotFoundException($"User with id {id} not found");

        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<UserModel> UpdateUserAsync(UserModel user, CancellationToken cancellationToken)
    {
        var usersEntity = await _userRepository.GetUsersByConditionsAsync(cancellationToken, u => u.Id == user.Id);

        if (usersEntity.FirstOrDefault() == null)
            throw new NotFoundException($"User with id {user.Id} not found");

        var userEntity = _mapper.Map<UserEntity>(user);
        userEntity = await _userRepository.UpdateAsync(userEntity, cancellationToken);

        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<UserModel> DeleteUserAsync(int id, CancellationToken cancellationToken)
    {
        var usersEntity = await _userRepository.GetUsersByConditionsAsync(cancellationToken,
            u => u.Id == id,
            includeProperties: "UserRole");
        var userEntity = usersEntity.FirstOrDefault();

        if (userEntity == null)
            throw new NotFoundException($"User with id {id} not found");

        await _userRepository.DeleteAsync(userEntity, cancellationToken);

        return _mapper.Map<UserModel>(userEntity);
    }

    public async Task<UserModel?> GetAvailableOperator(CancellationToken cancellationToken)
    {
        var operatorsEntityByCondition = await _userRepository.GetUsersByConditionsAsync(cancellationToken,
            u => u.UserRole.Name == "Operator",
            u => u.OrderBy(users => users.Tickets!.Count),
            "Tickets");
        var operatorEntity = operatorsEntityByCondition.FirstOrDefault();

        return operatorEntity == null ? null : _mapper.Map<UserModel>(operatorEntity);
    }
}