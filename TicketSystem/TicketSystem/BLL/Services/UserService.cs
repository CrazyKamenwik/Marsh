﻿using AutoMapper;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Repositories.Abstractions;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
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
            var usersEntity = await _userRepository.GetAllAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UserModel>>(usersEntity);
        }

        public async Task<UserModel?> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var userEntityByCondition =
                await _userRepository.GetUsersByConditionsAsync(cancellationToken, user => user.Id == id);
            var userEntity = userEntityByCondition.FirstOrDefault();

            return userEntity == null ? null : _mapper.Map<UserModel>(userEntity);
        }

        public async Task<UserModel?> UpdateUserAsync(UserModel user, CancellationToken cancellationToken)
        {
            var userEntity = _mapper.Map<UserEntity>(user);
            userEntity = await _userRepository.UpdateAsync(userEntity, cancellationToken);
            return userEntity == null ? null : _mapper.Map<UserModel>(userEntity);
        }

        public async Task<UserModel?> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {

            var userEntity = await _userRepository.DeleteAsync(id, cancellationToken);
            return userEntity == null ? null : _mapper.Map<UserModel>(userEntity);
        }

        public async Task<UserModel?> GetNotBusyOperator(CancellationToken cancellationToken)
        {
            var operatorsEntityByCondition = await _userRepository.GetUsersByConditionsAsync(cancellationToken, includeProperties: "Tickets",
                orderBy: u => u.OrderBy(users => users.Tickets));
            var operatorEntity = operatorsEntityByCondition.FirstOrDefault();
            return operatorEntity == null ? null : _mapper.Map<UserModel>(operatorEntity);
        }

    }
}