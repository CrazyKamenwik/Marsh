using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Models;
using TicketSystem.ViewModels.Messages;

namespace TicketSystem.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly ILogger<MessageController> _logger;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    private readonly IValidator<ShortMessageViewModel> _validator;

    public MessageController(IMessageService messageService, ILogger<MessageController> logger, IMapper mapper, IValidator<ShortMessageViewModel> validator)
    {
        _validator = validator;
        _mapper = mapper;
        _messageService = messageService;
        _logger = logger;
    }

    [HttpPost("{userId}")]
    public async Task<MessageViewModel> Post(int userId, ShortMessageViewModel shortMessage,
        CancellationToken cancellationToken)
    {
        await _validator.ValidateAndThrowAsync(shortMessage, cancellationToken);

        var messageModel = _mapper.Map<Message>(shortMessage);
        messageModel.UserId = userId;

        _logger.LogInformation("{JsonConvert.SerializeObject(message)}", JsonConvert.SerializeObject(messageModel));

        messageModel = await _messageService.AddMessageAsync(messageModel, cancellationToken);

        return _mapper.Map<MessageViewModel>(messageModel);
    }
}