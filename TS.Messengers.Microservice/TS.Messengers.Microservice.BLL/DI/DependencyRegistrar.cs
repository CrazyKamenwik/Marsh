using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TS.Messengers.Microservice.DAL.DI;

namespace TS.Messengers.Microservice.BLL.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddBusinessLogicLayerServices(this IServiceCollection serviceCollection,
IConfiguration configuration)
    {
        serviceCollection.AddDataAccessLayerServices(configuration);
        return serviceCollection;
    }
}