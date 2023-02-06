using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly Logger<TicketController> _logger;

        public TicketController(Logger<TicketController> logger)
        {
            _context = new ApplicationContext();
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<Ticket>> Get()
        {
            _logger.LogDebug("Get all tickets");
            var tickets = await _context.Tickets.ToListAsync();
            return tickets;
        }

        [HttpGet("{id}")]
        public async Task<Ticket?> Get(int id)
        {
            _logger.LogDebug("Find ticket by id {id}", id);
            return await _context.Tickets.FindAsync(id);
        }
    }
}