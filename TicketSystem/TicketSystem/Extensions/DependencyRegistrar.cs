using TicketSystem.Data;
using TicketSystem.Services;

namespace TicketSystem.Extensions
{
    public static class DependencyRegistrar
    {
        public static void AddBusinessLogicLayerServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddDbContext<ApplicationContext>();
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ITicketService, TicketService>();
            serviceCollection.AddTransient<IMessageService, MessageService>();
        }
    }
}
