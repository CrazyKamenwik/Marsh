using System.Linq.Expressions;
using TicketSystem.DAL.Entities;

namespace TicketSystem.DAL.Repositories.Abstractions
{
    public interface ITicketRepository
    {
        Task<TicketEntity> CreateAsync(TicketEntity ticket, CancellationToken cancellationToken);
        Task<TicketEntity?> UpdateAsync(TicketEntity ticket, CancellationToken cancellationToken);
        Task<TicketEntity?> DeleteAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<TicketEntity>> GetTicketsByConditionsAsync(CancellationToken cancellationToken,
            Expression<Func<TicketEntity, bool>>? filter = null,
            Func<IQueryable<TicketEntity>, IOrderedQueryable<TicketEntity>>? orderBy = null,
            string includeProperties = "");
        Task SaveAsync();
    }
}
