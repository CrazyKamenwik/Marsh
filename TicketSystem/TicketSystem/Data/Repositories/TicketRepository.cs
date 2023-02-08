using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicketSystem.Data.Models;
using TicketSystem.Data.Models.Enums;
using TicketSystem.Data.Repositories.Abstractions;

namespace TicketSystem.Data.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationContext _context;

        public TicketRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(cancellationToken);

            return ticket;
        }

        public async Task<Ticket?> UpdateAsync(Ticket ticket, CancellationToken cancellationToken)
        {
            var ticketExist = _context.Tickets.Any(x => x.Id == ticket.Id);
            if (!ticketExist)
                return null;

            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken); // DbUpdateConcurrencyException

            return ticket;
        }

        public async Task<Ticket?> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return null;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync(cancellationToken);

            return ticket;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _context.Tickets.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByConditionsAsync(CancellationToken cancellationToken = default,
            Expression<Func<Ticket, bool>>? filter = null,
            Func<IQueryable<Ticket>, IOrderedQueryable<Ticket>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<Ticket> query = _context.Tickets;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync(cancellationToken);
            }
            else
            {
                return await query.ToListAsync(cancellationToken);
            }
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
