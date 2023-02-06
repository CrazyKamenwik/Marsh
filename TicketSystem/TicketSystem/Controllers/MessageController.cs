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

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> Post(int userId, Message message)
        {
            var addResult = await _messageService.AddMessageAsync(userId, message);
            return addResult ? Ok() : NotFound();
        }
    }
}
