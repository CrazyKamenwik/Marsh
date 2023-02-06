using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicketSystem.Data;
using TicketSystem.Data.Models;

namespace TicketSystem.Data.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationContext _context;

        public TicketRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Ticket> CreateAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket?> UpdateAsync(int id, Ticket ticket)
        {
            var ticketExist = _context.Tickets.Any(x => x.Id == id);
            if (!ticketExist)
                return null;

            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // DbUpdateConcurrencyException

            return ticket;
        }

        public async Task<Ticket?> DeleteAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return null;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.AsNoTracking().ToListAsync();
        }

        public IQueryable<Ticket> GetUsersByConditionsAsync(Expression<Func<Ticket, bool>> conditions)
        {
            return _context.Tickets.AsNoTracking().Where(conditions);
        }

        public async Task CloseOpenTickets(int minutesToClose)
        {
            var tickets = _context.Tickets.Where(t => t.TicketStatus == TicketStatus.Open &&
                                                      t.Messages.OrderByDescending(m => m.CreatedAt)
                                                          .First().User.UserRole == UserRole.Operator)
                .ToList();

            foreach (var ticket in tickets)
            {
                if (ticket.CreatedAt.AddMinutes(minutesToClose) < DateTime.Now)
                    ticket.TicketStatus = TicketStatus.Closed;
            }

            await _context.SaveChangesAsync();
        }
    }
}
