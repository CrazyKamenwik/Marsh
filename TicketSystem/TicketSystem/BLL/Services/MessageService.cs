using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using TicketSystem.BLL.Models;
using TicketSystem.BLL.Models.Enums;
using TicketSystem.BLL.Services.Abstractions;

namespace TicketSystem.BLL.Services
{
    public class MessageService : IMessageService
    {
        private readonly IUserService _userService;
        private readonly ITicketService _ticketService;

        public MessageService(IUserService userService, ITicketService ticketService)
        {
            _userService = userService;
            _ticketService = ticketService;
        }

        // TODO #2: Change that method (SOLID)
        // Todo #4: Create message repo
        public async Task<MessageModel?> AddMessageAsync(MessageModel message, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(message.UserId, cancellationToken);
            if (user == null)
                return null;

            TicketModel? ticket;
            var freeOperator = await _userService.GetNotBusyOperator(cancellationToken);

            // If the user has an open ticket, the message will be written into it
            if (user.Tickets != null && user.Tickets!.Count != 0 && user.Tickets.OrderByDescending(t => t.CreatedAt).First().TicketStatus == TicketStatusEnumModel.Open)
            {
                ticket = user.Tickets!.OrderByDescending(t => t.CreatedAt).First();
            }
            else
            {
                ticket = new TicketModel()
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
}