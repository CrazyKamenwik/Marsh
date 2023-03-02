using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using TS.MailService.Infrastructure.Abstraction;
using TS.MailService.Infrastructure.Abstraction.Repository;
using TS.MailService.Infrastructure.Entities;

namespace TS.MailService.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : IBaseEntity
{
    private readonly IMongoCollection<T> _collection;

    public GenericRepository(IMongoClient client, IConfiguration configuration)
    {
        _collection = client.GetDatabase(configuration["MongoDb:Database"]).GetCollection<T>(typeof(T).Name);
    }

    public async Task Create(T entity, CancellationToken cancellationToken)
    {
        await _collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken)
    {
        return await _collection.Find("{}").ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<T> Get(Guid id, CancellationToken cancellationToken)
    {
        return await _collection
            .Find(e => e.Id == id)
            .FirstAsync(cancellationToken);
    }
}