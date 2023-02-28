using AutoMapper;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Enums;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.BLL.MessagesStrategy;

public class UserMessageStrategy : IMessageStrategy
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly ITicketService _ticketService;

    public UserMessageStrategy(ITicketService ticketService,
        IGenericRepository<MessageEntity> messageRepository, IMapper mapper)
    {
        _mapper = mapper;
        _ticketService = ticketService;
        _messageRepository = messageRepository;
    }

    public bool IsApplicable(string userRole)
    {
        return userRole == RolesConstants.User;
    }

    public async Task<Message> AddMessage(Message message, User user, CancellationToken cancellationToken)
    {
        await SetOpenTicketToMessage(message, user, cancellationToken);

        var messageEntity = _mapper.Map<MessageEntity>(message);

        await _messageRepository.Create(messageEntity, cancellationToken);

        return _mapper.Map<Message>(messageEntity);
    }

    private async Task SetOpenTicketToMessage(Message message, User user, CancellationToken cancellationToken)
    {
        Ticket ticket;

        if (IsUserHasOpenTickets(user))
        {
            ticket = user.Tickets!.First(t => t.TicketStatus == TicketStatusEnumModel.Open);
        }
        else
        {
            ticket = new Ticket(user.Id);
            ticket = await _ticketService.AddTicket(ticket, cancellationToken);
        }

        message.TicketId = ticket.Id;
    }

    private static bool IsUserHasOpenTickets(User user)
    {
        return user.Tickets?.Any(t => t.TicketStatus == TicketStatusEnumModel.Open) ?? false;
    }
}