using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Entities;
using TicketSystem.DAL.Repositories;
using TicketSystem.DAL.Repositories.Abstractions;

namespace TicketSystem.DAL.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddDataAccessLevelServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddDbContext<ApplicationContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DbConnection")));

        serviceCollection.AddTransient<ITicketRepository, TicketRepository>();
        serviceCollection.AddTransient<IUserRepository, UserRepository>();

        serviceCollection.AddTransient<IGenericRepository<UserEntity>, GenericRepository<UserEntity>>();
        serviceCollection.AddTransient<IGenericRepository<TicketEntity>, GenericRepository<TicketEntity>>();
        serviceCollection.AddTransient<IGenericRepository<MessageEntity>, GenericRepository<MessageEntity>>();

        return serviceCollection;
    }
}