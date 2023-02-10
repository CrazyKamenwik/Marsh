using TicketSystem.BLL.Models;
using TicketSystem.BLL.Models.Enums;
using TicketSystem.BLL.Services.Abstractions;

namespace TicketSystem.BLL.Services;

public class MessageService : IMessageService
{
    private readonly ITicketService _ticketService;
    private readonly IUserService _userService;

    public MessageService(IUserService userService, ITicketService ticketService)
    {
        _userService = userService;
        _ticketService = ticketService;
    }

    public async Task<MessageModel> AddMessageAsync(MessageModel message, CancellationToken cancellationToken)
    {
        var user = await _userService.GetUserByIdAsync(message.UserId, cancellationToken);

        TicketModel? ticket;
        var freeOperator = await _userService.GetAvailableOperator(cancellationToken);

        // If the user has an open ticket, the message will be written into it
        if (user.Tickets != null && user.Tickets!.Count != 0 &&
            user.Tickets.OrderByDescending(t => t.CreatedAt).First().TicketStatus == TicketStatusEnumModel.Open)
        {
            ticket = user.Tickets!.OrderByDescending(t => t.CreatedAt).First();
        }
        else
        {
            ticket = new TicketModel
            {
                TicketCreatorId = user.Id,
                OperatorId = freeOperator?.Id ?? null,
                CreatedAt = DateTime.UtcNow
            };
            ticket = await _ticketService.AddTicketAsync(ticket, cancellationToken);
        }

        message.TicketId = ticket.Id;
        message.CreatedAt = DateTime.UtcNow;
        ticket.Messages.Add(message);
        ticket = await _ticketService.UpdateTicketAsync(ticket, cancellationToken);

        return ticket!.Messages.Last();
    }
}