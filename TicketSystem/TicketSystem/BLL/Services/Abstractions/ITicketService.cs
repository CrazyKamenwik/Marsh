using TicketSystem.DAL.Entities;

namespace TicketSystem.BLL.Services.Abstractions
{
    public interface ITicketService
    {
        Task<TicketEntity> AddTicketAsync(TicketEntity ticket, CancellationToken cancellationToken);
        Task<TicketEntity?> GetTicketByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<TicketEntity>> GetTicketsAsync(CancellationToken cancellationToken);
        Task<TicketEntity?> UpdateTicketAsync(TicketEntity ticket, CancellationToken cancellationToken);
        Task<TicketEntity?> DeleteTicketAsync(int id, CancellationToken cancellationToken);
        Task CloseOpenTickets(CancellationToken cancellationToken = default);
    }
}