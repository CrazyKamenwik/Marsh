using Microsoft.AspNetCore.Mvc;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketContoller : ControllerBase
    {
        private readonly ILogger<TicketContoller> _logger;
        private readonly ApplicationContext _context;

        public TicketContoller(ILogger<TicketContoller> logger)
        {
            _logger = logger;
            _context = new ApplicationContext();
        }

        [HttpGet(Name = "GetTickets")]
        public IEnumerable<Ticket> Get()
        {
             return _context.Tickets.ToList();
        }
    }
}