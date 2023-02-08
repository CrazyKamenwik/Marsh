using TicketSystem.DAL.Entities;

namespace TicketSystem.BLL.Services.Abstractions
{
    public interface IMessageService
    {
        Task<bool> AddMessageAsync(MessageEntity message, CancellationToken cancellationToken);
    }
}
