using System.Linq.Expressions;
using TicketSystem.Data.Models;

namespace TicketSystem.Data.Repositories
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateAsync(Ticket ticket);
        Task<Ticket?> UpdateAsync(int id, Ticket ticket);
        Task<Ticket?> DeleteAsync(int id);
        Task<IEnumerable<Ticket>> GetAllAsync();
        IQueryable<Ticket> GetUsersByConditionsAsync(Expression<Func<Ticket, bool>> conditions);
        Task<int> CloseOpenTickets(int minutesToClose);
    }
}
