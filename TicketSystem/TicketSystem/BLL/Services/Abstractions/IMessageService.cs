using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Services.Abstractions;

public interface IMessageService
{
    Task<MessageModel> AddMessageAsync(MessageModel message, CancellationToken cancellationToken);
}