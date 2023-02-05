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

        public UserController()
        {
            _context = new ApplicationContext();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _context.Users.ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _context.Users.FindAsync(id);

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            var userDB = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return Ok(userDB);
        }

        [HttpPut("id")]
        public async Task<IActionResult> Put(int id, User user)
        {
            if(id != user.Id)
                return BadRequest();

            bool userExist = _context.Users.Any(x => x.Id == id);
            if(!userExist)
                return NotFound();

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync(); //DbUpdateConcurrencyException

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if(user == null)
                return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }
    }
}
