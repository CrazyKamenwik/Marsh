using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<UserEntity>> Get(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get all users");
            var users = await _userService.GetUsersAsync(cancellationToken);

            return users;
        }

        [HttpGet("{id}")]
        public async Task<UserEntity?> Get(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get user by id {id}", id);

            return await _userService.GetUserByIdAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<UserEntity> Post(UserEntity user, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{JsonConvert.SerializeObject(user)}", JsonConvert.SerializeObject(user));

            return await _userService.AddUserAsync(user, cancellationToken);
        }

        [HttpPut("{id}")]
        public async Task<UserEntity?> Put(int id, UserEntity user, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{JsonConvert.SerializeObject(user)}", JsonConvert.SerializeObject(user));
            user.Id = id;
            return await _userService.UpdateUserAsync(user, cancellationToken);
        }

        [HttpDelete("{id}")]
        public async Task<UserEntity?> Delete(int id, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Delete user by id {id}", id);
            return await _userService.DeleteUserAsync(id, cancellationToken);
        }
    }
}
