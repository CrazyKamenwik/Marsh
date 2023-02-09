using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Services.Abstractions
{
    public interface ITicketService
    {
        Task<TicketModel> AddTicketAsync(TicketModel ticket, CancellationToken cancellationToken);
        Task<TicketModel?> GetTicketByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<TicketModel>> GetTicketsAsync(CancellationToken cancellationToken);
        Task<TicketModel?> UpdateTicketAsync(TicketModel ticketModel, CancellationToken cancellationToken);
        Task<TicketModel?> DeleteTicketAsync(int id, CancellationToken cancellationToken);
        Task CloseOpenTickets(CancellationToken cancellationToken = default);
        // TODO #3: Create method that return last open ticket or create new
    }
}