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
        private readonly ILogger<MessageController> _logger;

        public MessageController(ILogger<MessageController> logger)
        {
            _logger = logger;
            _context = new ApplicationContext();
        }

        [HttpPost]
        public async Task<Message> Post(Message message)
        {
            _logger.LogDebug("Post message");
            await _context.Messages.AddAsync(message);
            await _context.SaveChangesAsync();

            return message;
        }
    }
}