using TicketSystem.Data.Models;

namespace TicketSystem.Services.Abstractions
{
    public interface ITicketService
    {
        Task<Ticket> AddTicketAsync(Ticket ticket);
        Task<Ticket?> GetTicketByIdAsync(int id);
        Task<IEnumerable<Ticket>> GetTicketsAsync();
        Task<Ticket?> UpdateTicketAsync(Ticket ticket);
        Task<Ticket?> DeleteTicketAsync(int id);
    }
}