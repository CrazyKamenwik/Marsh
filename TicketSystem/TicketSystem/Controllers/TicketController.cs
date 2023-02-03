using Microsoft.AspNetCore.Mvc;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketContoller : ControllerBase
    {
        private readonly ILogger<TicketContoller> _logger;

        public TicketContoller(ILogger<TicketContoller> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetTickets")]
        public IEnumerable<Ticket> Get()
        {
            return Enumerable.Empty<Ticket>();
        }
    }
}