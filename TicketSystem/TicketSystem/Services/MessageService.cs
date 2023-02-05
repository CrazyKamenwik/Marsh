using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Services
{
    public class MessageService : IMessageService
    {
        private readonly ApplicationContext _context;
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;

        public MessageService(ApplicationContext context, ITicketService ticketService, IUserService userService)
        {
            _context = context;
            _ticketService = ticketService;
            _userService = userService;
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
                ticket = new Ticket(_context, user, freeOperator);
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
