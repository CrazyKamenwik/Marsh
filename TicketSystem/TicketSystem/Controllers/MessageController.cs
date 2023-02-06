using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly ApplicationContext _context;

        public MessageController()
        {
            _context = new ApplicationContext();
        }

        [HttpPost]
        public async Task<IActionResult> Post(Message message)
        {
            var messageDB = await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return Ok(messageDB);
        }
    }
}