using System.Reflection;
using TicketSystem.BLL.Services;
using TicketSystem.BLL.Services.Abstractions;
using TicketSystem.DAL.DI;

namespace TicketSystem.BLL.DI
{
    public static class DependencyRegistrar
    {
        public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddAutoMapper(Assembly.GetExecutingAssembly());

            serviceCollection.AddDataAccessLevelServices(configuration);
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ITicketService, TicketService>();
            serviceCollection.AddTransient<IMessageService, MessageService>();

            return serviceCollection;
        }
    }
}
