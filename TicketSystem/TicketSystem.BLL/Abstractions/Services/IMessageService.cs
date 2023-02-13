using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Abstractions.Services;

public interface IMessageService
{
    Task<Message> AddMessageAsync(Message message, CancellationToken cancellationToken);
}