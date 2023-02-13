using AutoMapper;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Constants;
using TicketSystem.BLL.Enums;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Abstractions;

namespace TicketSystem.BLL.Abstractions.MessagesStrategy;

public abstract class MessageStrategy
{
    protected readonly IMapper Mapper;
    protected readonly IGenericRepository<MessageEntity> MessageRepository;
    protected readonly ITicketService TicketService;
    protected readonly IUserService UserService;

    protected MessageStrategy(IUserService userService, ITicketService ticketService,
        IGenericRepository<MessageEntity> messageRepository, IMapper mapper)
    {
        UserService = userService;
        TicketService = ticketService;
        MessageRepository = messageRepository;
        Mapper = mapper;
    }

    public virtual async Task<Message> AddMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var user = await UserService.GetUserByIdAsync(message.UserId, cancellationToken);

        switch (user.UserRole.Name)
        {
            case RolesConstants.User:
                await SetOpenTicketToMessageAsync(message, user, cancellationToken);
                break;
            case RolesConstants.Operator:
                await TicketService.GetTicketByIdAsync(message.TicketId,
                    cancellationToken); // throw exception if ticket not exist
                break;
        }

        var messageEntity = Mapper.Map<MessageEntity>(message);
        await MessageRepository.CreateAsync(messageEntity, cancellationToken);

        return Mapper.Map<Message>(message);
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
            ticket = await TicketService.AddTicketAsync(ticket, cancellationToken);
        }

        message.TicketId = ticket.Id;
    }

    private static bool IsUserHasOpenTickets(User user)
    {
        return user.Tickets?.Any(t => t.TicketStatus == TicketStatusEnumModel.Open) ?? false;
    }
}