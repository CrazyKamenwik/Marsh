using AutoMapper;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.MessagesStrategy;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Abstractions;

namespace TicketSystem.BLL.Services;

public class MessageService : IMessageService
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;
    private MessageStrategy _messageStrategy;

    public MessageService(IGenericRepository<MessageEntity> messageRepository, IUserService userService,
        ITicketService ticketService, IMapper mapper, MessageStrategy messageStrategy)
    {
        _messageRepository = messageRepository;
        _userService = userService;
        _ticketService = ticketService;
        _mapper = mapper;
        _messageStrategy = messageStrategy;
    }

    public async Task<Message> AddMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(message.UserId, cancellationToken);

        switch (user.UserRole.Name)
        {
            case RolesConstants.User:
                _messageStrategy = new UserMessageStrategy(_userService, _ticketService, _messageRepository, _mapper);
                break;
            case RolesConstants.Operator:
                _messageStrategy =
                    new OperatorMessageStrategy(_userService, _ticketService, _messageRepository, _mapper);
                break;
        }

        message = await _messageStrategy.AddMessageAsync(message, cancellationToken);
        return _mapper.Map<Message>(message);
    }
}