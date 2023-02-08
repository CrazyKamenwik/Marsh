using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.ViewModels.Users;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, ILogger<UserController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<UserVm>> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all users");
            var usersModel = await _userService.GetUsersAsync(cancellationToken);

            return _mapper.Map<IEnumerable<UserVm>>(usersModel);
        }

        [HttpGet("{id}")]
        public async Task<UserVm?> Get(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get user by id {id}", id);

            var userModel = await _userService.GetUserByIdAsync(id, cancellationToken);

            return userModel == null ? null : _mapper.Map<UserVm>(userModel);
        }

        [HttpPost]
        public async Task<UserVm> Post(ShortUser shortUser, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{JsonConvert.SerializeObject(user)}", JsonConvert.SerializeObject(shortUser));
            
            var userModel = _mapper.Map<UserModel>(shortUser);
            await _userService.AddUserAsync(userModel, cancellationToken);

            return _mapper.Map<UserVm>(userModel);
        }

        [HttpPut("{id}")]
        public async Task<UserVm?> Put(int id, ShortUser shortUser, CancellationToken cancellationToken)
        {
            var userModel = _mapper.Map<UserModel>(shortUser);
            userModel.Id = id;

            _logger.LogInformation("{JsonConvert.SerializeObject(user)}", JsonConvert.SerializeObject(userModel));

            var userModelFromBll = await _userService.UpdateUserAsync(userModel, cancellationToken);
            return userModelFromBll == null ? null : _mapper.Map<UserVm>(userModelFromBll);
        }

        [HttpDelete("{id}")]
        public async Task<UserVm?> Delete(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delete user by id {id}", id);
            var userModel = await _userService.DeleteUserAsync(id, cancellationToken);
            return userModel == null ? null : _mapper.Map<UserVm>(userModel);
        }
    }
}
