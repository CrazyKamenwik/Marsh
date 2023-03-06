namespace TS.MailService.Infrastructure.Abstraction.Repository;

public interface IGenericRepository<T> where T : IBaseEntity
{
    public Task Create(T entity, CancellationToken cancellationToken);
    public Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    public Task<T> Get(Guid id, CancellationToken cancellationToken);
}