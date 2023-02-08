using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TicketSystem.DAL.Repositories.Abstractions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.DAL.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationContext _context;

        public TicketRepository(ApplicationContext context)
        {
            _context = context;
        }

        // Change return type to Task, because ticket is trackable
        public async Task<TicketEntity> CreateAsync(TicketEntity ticket, CancellationToken cancellationToken)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync(cancellationToken);

            return ticket;
        }

        public async Task<TicketEntity?> UpdateAsync(TicketEntity ticket, CancellationToken cancellationToken)
        {
            var ticketExist = _context.Tickets.Any(x => x.Id == ticket.Id);
            if (!ticketExist)
                return null;

            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken); // DbUpdateConcurrencyException

            return ticket;
        }

        public async Task<TicketEntity?> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            var ticket = await _context.Tickets
                .Include(t => t.TicketCreator)
                .Include(t => t.Operator)
                .Include(t => t.Messages)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (ticket == null)
                return null;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync(cancellationToken);

            return ticket;
        }

        public async Task<IEnumerable<TicketEntity>> GetTicketsByConditionsAsync(CancellationToken cancellationToken = default,
            Expression<Func<TicketEntity, bool>>? filter = null,
            Func<IQueryable<TicketEntity>, IOrderedQueryable<TicketEntity>>? orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TicketEntity> query = _context.Tickets;

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
