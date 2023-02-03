using Microsoft.AspNetCore.Mvc;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<Ticket> Get()
        {
            return Enumerable.Empty<Ticket>();
        }
    }
}
