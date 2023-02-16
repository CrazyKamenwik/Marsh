using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Abstractions.Services;

public interface ITicketService
{
    Task<Ticket> AddTicket(Ticket ticket, CancellationToken cancellationToken);
    Task<Ticket> GetTicketById(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Ticket>> GetTickets(CancellationToken cancellationToken);
    Task<Ticket> UpdateTicket(Ticket ticketModel, CancellationToken cancellationToken);
    Task DeleteTicket(int id, CancellationToken cancellationToken);
    Task CloseOpenTickets(CancellationToken cancellationToken = default);
}