using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using TicketSystem.Data.Models;
using TicketSystem.Services;

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
        public async Task<IEnumerable<User>> Get()
        {
            _logger.LogDebug("Get all users");
            var users = await _userService.GetUsersAsync();

            return users;
        }

        [HttpGet("{id}")]
        public async Task<User?> Get(int id)
        {
            _logger.LogDebug("Get user by id {id}", id);

            return await _userService.GetUserByIdAsync(id);
        }

        [HttpPost]
        public async Task<User> Post(User user)
        {
            _logger.LogDebug("Put new user");

            return await _userService.AddUserAsync(user);
        }

        [HttpPut("{id}")]
        public async Task<User?> Put(int id, User user)
        {
            _logger.LogDebug("Put user by id {id}", id);
            if (id != user.Id)
                return null;

            bool userExist = _context.Users.Any(x => x.Id == id);
            if (!userExist)
                return null;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(); //DbUpdateConcurrencyException

            return await _userService.UpdateUserAsync(id, user);
        }

        [HttpDelete("{id}")]
        public async Task<User?> Delete(int id)
        {
            _logger.LogDebug("Delete user by id {id}", id);
            var user = await _context.Users.FindAsync(id);
            if(user == null)
                return null;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return await _userService.DeleteUserAsync(id);
        }
    }
}
