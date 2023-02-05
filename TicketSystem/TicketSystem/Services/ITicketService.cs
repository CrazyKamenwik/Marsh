using TicketSystem.Models;

namespace TicketSystem.Services
{
    public interface ITicketService
    {
        Task<Ticket> AddTicketAsync(Ticket ticket);
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<Ticket?> UpdateTicketAsync(int id, Ticket ticket);
        Task<Ticket?> DeleteTicketAsync(int id);
    }
}