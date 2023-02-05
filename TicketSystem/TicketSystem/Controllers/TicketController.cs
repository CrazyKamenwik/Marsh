using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketContoller : ControllerBase
    {
        private readonly ApplicationContext _context;

        public TicketContoller()
        {
            _context = new ApplicationContext();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tickets = await _context.Tickets.ToListAsync();

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return NotFound();

            return Ok(ticket);
        }
    }
}