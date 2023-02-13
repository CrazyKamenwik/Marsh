using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Entities.Abstractions;
using TicketSystem.DAL.Repositories;

namespace TicketSystem.DAL.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddDataAccessLevelServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DbConnection")));

        serviceCollection.AddTransient<IGenericRepository<UserEntity>, GenericRepository<UserEntity>>();
        serviceCollection.AddTransient<IGenericRepository<TicketEntity>, GenericRepository<TicketEntity>>();
        serviceCollection.AddTransient<IGenericRepository<MessageEntity>, GenericRepository<MessageEntity>>();

        return serviceCollection;
    }
}