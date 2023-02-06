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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userService.GetUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            var userDB = await _userService.AddUserAsync(user);
            return Ok(userDB);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, User user)
        {
            var userDB = await _userService.UpdateUserAsync(id, user);
            return Ok(userDB);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.DeleteUserAsync(id);
            return Ok(user);
        }
    }
}
