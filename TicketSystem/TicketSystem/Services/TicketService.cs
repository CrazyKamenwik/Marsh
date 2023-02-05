using Microsoft.EntityFrameworkCore;
using TicketSystem.Data;
using TicketSystem.Models;

namespace TicketSystem.Services
{
    public class TicketService : ITicketService
    {
        private readonly ApplicationContext _context;

        public TicketService(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Ticket> AddTicketAsync(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task<IEnumerable<Ticket>> GetTicketsAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket)
        {
            if (id != ticket.Id)
                return null;

            bool ticketExist = _context.Tickets.Any(x => x.Id == id);
            if (!ticketExist)
                return null;

            _context.Entry(ticket).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // DbUpdateConcurrencyException

            return ticket;
        }

        public async Task<Ticket?> DeleteTicketAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
                return null;

            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }
    }
}