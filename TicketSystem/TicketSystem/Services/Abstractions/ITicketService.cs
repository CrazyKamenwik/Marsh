using TicketSystem.Data.Models;

namespace TicketSystem.Services.Abstractions
{
    public interface ITicketService
    {
        Task<Ticket> AddTicketAsync(Ticket ticket, CancellationToken cancellationToken);
        Task<Ticket?> GetTicketByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Ticket>> GetTicketsAsync(CancellationToken cancellationToken);
        Task<Ticket?> UpdateTicketAsync(Ticket ticket, CancellationToken cancellationToken);
        Task<Ticket?> DeleteTicketAsync(int id, CancellationToken cancellationToken);
        Task CloseOpenTickets(CancellationToken cancellationToken = default);
    }
}