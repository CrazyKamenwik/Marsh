using TicketSystem.Data.Models;

namespace TicketSystem.Services
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

        public async Task<bool> AddMessageAsync(int userId, Message message)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null)
                return false;

            Ticket ticket;
            var freeOperator = await _userService.GetFreeOperator();

            // If the user has an open ticket, the message will be written into it
            if (user.Tickets == null || user.Tickets.Count == 0 && user.Tickets.OrderByDescending(t => t.CreatedAt).First().TicketStatus == TicketStatus.Open)
            {
                ticket = user.Tickets!.OrderByDescending(t => t.CreatedAt).First();
            }
            else
            {
                ticket = new Ticket(_ticketService, user, freeOperator);
                await _ticketService.AddTicketAsync(ticket);
            }

            message.Ticket = ticket;
            message.CreatedAt = DateTime.UtcNow;
            ticket.Messages!.Add(message);
            await _ticketService.UpdateTicketAsync(ticket.Id, ticket);

            return true;
        }
    }
}
