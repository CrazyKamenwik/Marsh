using Microsoft.AspNetCore.Mvc;
using TicketSystem.Data.Models;
using TicketSystem.Services;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger _logger;

        public MessageController(IMessageService messageService, ILogger logger)
        {
            _messageService = messageService;
            _logger = logger;
            _context = new ApplicationContext();
        }

        [HttpPost("{userId}")]
        public async Task<Message> Post(int userId, Message message)
        {
            _logger.LogDebug("Post message bu userId {userId}", userId);
            await _messageService.AddMessageAsync(userId, message);

            return message;
        }
    }
}
