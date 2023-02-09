using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.Entities;
using TicketSystem.ViewModels.Messages;

namespace TicketSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly ILogger<MessageController> _logger;
        private readonly IMapper _mapper;

        public MessageController(IMessageService messageService, ILogger<MessageController> logger, IMapper mapper)
        {
            _mapper = mapper;
            _messageService = messageService;
            _logger = logger;
        }

        [HttpPost("{userId}")]
        public async Task<MessageVm?> Post(int userId, ShortMessage shortMessage, CancellationToken cancellationToken)
        {
            var messageModel = _mapper.Map<MessageModel>(shortMessage);
            messageModel.UserId = userId;

            _logger.LogInformation("{JsonConvert.SerializeObject(message)}", JsonConvert.SerializeObject(messageModel));

            messageModel = await _messageService.AddMessageAsync(messageModel, cancellationToken);

            return messageModel == null ? null : _mapper.Map<MessageVm>(messageModel);
        }
    }
}