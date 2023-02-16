using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Abstractions.Services;

public interface ITicketService
{
    Task<Ticket> AddTicketAsync(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket> GetTicketByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Ticket>> GetTicketsAsync(CancellationToken cancellationToken);
    Task<Ticket> UpdateTicketAsync(Ticket ticketModel, CancellationToken cancellationToken);
    Task DeleteTicketAsync(int id, CancellationToken cancellationToken);
    Task CloseOpenTickets(CancellationToken cancellationToken = default);
}