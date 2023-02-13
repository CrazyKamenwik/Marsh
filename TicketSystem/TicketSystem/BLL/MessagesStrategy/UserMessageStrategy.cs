using AutoMapper;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.Enums;
using TicketSystem.BLL.Models;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Abstractions;

namespace TicketSystem.BLL.MessagesStrategy;

public class UserMessageStrategy : MessageStrategy
{
    public UserMessageStrategy(IUserService userService, ITicketService ticketService,
        IGenericRepository<MessageEntity> messageRepository, IMapper mapper)
        : base(userService, ticketService, messageRepository, mapper)
    {
    }

    public override async Task<Message> AddMessageAsync(Message message, CancellationToken cancellationToken)
    {
        var user = await UserService.GetUserByIdAsync(message.UserId, cancellationToken);
        await SetOpenTicketToMessageAsync(message, user, cancellationToken);
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