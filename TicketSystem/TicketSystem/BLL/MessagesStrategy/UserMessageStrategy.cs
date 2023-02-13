using AutoMapper;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Enums;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Abstractions;

namespace TicketSystem.BLL.MessagesStrategy;

public class UserMessageStrategy : IMessageStrategy
{
    private readonly IMapper _mapper;
    private readonly IGenericRepository<MessageEntity> _messageRepository;
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;

    public UserMessageStrategy(IUserService userService, ITicketService ticketService,
        IGenericRepository<MessageEntity> messageRepository, IMapper mapper)
    {
        _mapper = mapper;
        _ticketService = ticketService;
        _messageRepository = messageRepository;
        _userService = userService;
    }

    public bool IsApplicable(string userRole)
    {
        return userRole == RolesConstants.User;
    }

    public async Task<Message> AddMessageAsync(Message message, User user, CancellationToken cancellationToken)
    {
        await SetOpenTicketToMessageAsync(message, user, cancellationToken);

        var messageEntity = _mapper.Map<MessageEntity>(message);

        await _messageRepository.CreateAsync(messageEntity, cancellationToken);

        return _mapper.Map<Message>(messageEntity);
    }

    private async Task SetOpenTicketToMessageAsync(Message message, User user, CancellationToken cancellationToken)
    {
        Ticket ticket;

        if (IsUserHasOpenTickets(user))
        {
            ticket = user.Tickets!.First(t => t.TicketStatus == TicketStatusEnumModel.Open);
        }
        else
        {
            ticket = new Ticket(user.Id);
            ticket = await _ticketService.AddTicketAsync(ticket, cancellationToken);
        }

        message.TicketId = ticket.Id;
    }

    private static bool IsUserHasOpenTickets(User user)
    {
        return user.Tickets?.Any(t => t.TicketStatus == TicketStatusEnumModel.Open) ?? false;
    }
}