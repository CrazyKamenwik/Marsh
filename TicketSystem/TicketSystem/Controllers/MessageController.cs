using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;

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
        public async Task<MessageEntity> Post(int userId, MessageEntity message, CancellationToken cancellationToken)
        {
            _logger.LogInformation("{JsonConvert.SerializeObject(message)}", JsonConvert.SerializeObject(message));
            await _messageService.AddMessageAsync(message, cancellationToken);
            message.UserId = userId;
            return message;
        }
    }
}
