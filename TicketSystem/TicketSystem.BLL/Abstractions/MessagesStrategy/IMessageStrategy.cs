using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Abstractions.MessagesStrategy;

public interface IMessageStrategy
{
    bool IsApplicable(string userRole);
    Task<Message> AddMessageAsync(Message message, User user, CancellationToken cancellationToken);
}