using TicketSystem.Data.DI;

namespace TicketSystem.Services.DI
{
    public static class DependencyRegistrar
    {
        public static void AddBusinessLogicLayerServices(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDataAccessLevelServices(configuration);
            serviceCollection.AddTransient<IUserService, UserService>();
            serviceCollection.AddTransient<ITicketService, TicketService>();
            serviceCollection.AddTransient<IMessageService, MessageService>();
        }
    }
}
