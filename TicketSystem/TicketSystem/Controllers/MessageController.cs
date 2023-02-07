using Microsoft.AspNetCore.Mvc;
using TicketSystem.Data.Models;
using TicketSystem.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;

        public MessageController(IMessageService messageService, ILogger<MessageController> logger)
        {
            _messageService = messageService;
            _logger = logger;
        }

        [HttpPost("{userId}")]
        public async Task<Message> Post(int userId, Message message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Post new message by userId {userId}, message: {JsonConvert.SerializeObject(message)}",userId, JsonConvert.SerializeObject(message));
            await _messageService.AddMessageAsync(message, cancellationToken);
            message.UserId = userId;
            return message;
        }
    }
}
