using AutoMapper;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.BLL.MessagesStrategy;

public class OperatorMessageStrategy : IMessageStrategy
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly ITicketService _ticketService;

    public OperatorMessageStrategy(ITicketService ticketService,
        IGenericRepository<MessageEntity> messageRepository, IMapper mapper)
    {
        _mapper = mapper;
        _ticketService = ticketService;
        _messageRepository = messageRepository;
    }

    public bool IsApplicable(string userRole)
    {
        return userRole == RolesConstants.Operator;
    }

    public async Task<Message> AddMessage(Message message, User user, CancellationToken cancellationToken)
    {
        await _ticketService.GetTicketById(message.TicketId, cancellationToken);

        var messageEntity = _mapper.Map<MessageEntity>(message);

        await _messageRepository.Create(messageEntity, cancellationToken);

        return _mapper.Map<Message>(messageEntity);
    }
}