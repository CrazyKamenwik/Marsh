using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Repositories.Abstractions;

namespace TicketSystem.DAL.Repositories;

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

    public async Task<TicketEntity> UpdateAsync(TicketEntity ticket, CancellationToken cancellationToken)
    {
        _context.Entry(ticket).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken); // DbUpdateConcurrencyException

        return ticket;
    }

    public async Task<TicketEntity> DeleteAsync(TicketEntity ticket, CancellationToken cancellationToken)
    {
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync(cancellationToken);

        return ticket;
    }

    public async Task<IEnumerable<TicketEntity>> GetTicketsByConditionsAsync(
        CancellationToken cancellationToken = default,
        Expression<Func<TicketEntity, bool>>? filter = null,
        Func<IQueryable<TicketEntity>, IOrderedQueryable<TicketEntity>>? orderBy = null,
        string includeProperties = "")
    {
        IQueryable<TicketEntity> query = _context.Tickets;

        if (filter != null) query = query.Where(filter);

        foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);

        if (orderBy != null)
            return await orderBy(query).ToListAsync(cancellationToken);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}