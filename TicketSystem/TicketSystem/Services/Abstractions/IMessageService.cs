using TicketSystem.Data.Models;

namespace TicketSystem.Services.Abstractions
{
    public interface IMessageService
    {
        Task<bool> AddMessageAsync(Message message, CancellationToken cancellationToken);
    }
}
