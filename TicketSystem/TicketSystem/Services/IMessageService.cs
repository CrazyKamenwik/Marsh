using TicketSystem.Data.Models;

namespace TicketSystem.Services
{
    public interface IMessageService
    {
        Task<bool> AddMessageAsync(int userId, Message message);
    }
}
