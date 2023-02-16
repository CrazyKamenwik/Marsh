using TicketSystem.BLL.Models;

namespace TicketSystem.BLL.Abstractions.MessagesStrategy;

public interface IMessageStrategy
{
    bool IsApplicable(string userRole);
    Task<Message> AddMessage(Message message, User user, CancellationToken cancellationToken);
}