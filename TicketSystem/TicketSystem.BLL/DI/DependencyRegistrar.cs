using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TicketSystem.BLL.Abstractions.MessagesStrategy;
using TicketSystem.BLL.Abstractions.Services;
using TicketSystem.BLL.MapperProfiles;
using TicketSystem.BLL.MessagesStrategy;
using TicketSystem.BLL.Services;
using TicketSystem.DAL.DI;

namespace TicketSystem.BLL.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        serviceCollection.AddAutoMapper(typeof(MapperProfile).GetTypeInfo().Assembly);

        serviceCollection.AddDataAccessLevelServices(configuration);
        serviceCollection.AddTransient<IUserService, UserService>();
        serviceCollection.AddTransient<ITicketService, TicketService>();
        serviceCollection.AddTransient<IMessageService, MessageService>();

        serviceCollection.AddTransient<IMessageStrategy, UserMessageStrategy>();
        serviceCollection.AddTransient<IMessageStrategy, OperatorMessageStrategy>();

        return serviceCollection;
    }
}