using System.Linq.Expressions;
using TicketSystem.Data.Models;

namespace TicketSystem.Data.Repositories.Abstractions
{
    public interface ITicketRepository
    {
        Task<Ticket> CreateAsync(Ticket ticket, CancellationToken cancellationToken);
        Task<Ticket?> UpdateAsync(Ticket ticket, CancellationToken cancellationToken);
        Task<Ticket?> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<Ticket>> GetAllAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Ticket>> GetTicketsByConditionsAsync(CancellationToken cancellationToken,
            Expression<Func<Ticket, bool>>? filter = null,
            Func<IQueryable<Ticket>, IOrderedQueryable<Ticket>>? orderBy = null,
            string includeProperties = "");
        Task SaveAsync();
    }
}
