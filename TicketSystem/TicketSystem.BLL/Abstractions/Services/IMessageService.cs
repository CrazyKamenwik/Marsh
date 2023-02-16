using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Abstractions.Services;

public interface IMessageService
{
    Task<Message> AddMessage(Message message, CancellationToken cancellationToken);
}