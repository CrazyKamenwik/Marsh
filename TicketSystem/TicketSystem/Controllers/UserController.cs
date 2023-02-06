using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _context = new ApplicationContext();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<User?> Get(int id)
        {
            _logger.LogDebug("Find user by id {id}", id);
            return await _context.Users.FindAsync(id);
        }

        [HttpPost]
        public async Task<User> Post(User user)
        {
            _logger.LogDebug("Post user");
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        [HttpPut("id")]
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

            return user;
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

            return user;
        }
    }
}
