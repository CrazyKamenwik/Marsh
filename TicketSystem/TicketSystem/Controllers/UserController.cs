using Microsoft.AspNetCore.Mvc;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly ApplicationContext _context;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            _context = new ApplicationContext();
        }

        [HttpGet(Name = "GetUser")]
        public IEnumerable<User> Get()
        {
            return _context.Users.ToList();
        }
    }
}
