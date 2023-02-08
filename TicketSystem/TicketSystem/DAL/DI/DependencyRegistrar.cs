using Microsoft.EntityFrameworkCore;
using TicketSystem.DAL.Repositories;
using TicketSystem.DAL.Repositories.Abstractions;

namespace TicketSystem.DAL.DI
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddDataAccessLevelServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DbConnection")));

            serviceCollection.AddTransient<ITicketRepository, TicketRepository>();
            serviceCollection.AddTransient<IUserRepository, UserRepository>();

            return serviceCollection;
        }
    }
}
