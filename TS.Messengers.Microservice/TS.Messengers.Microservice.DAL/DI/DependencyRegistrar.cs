using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TS.Messengers.Microservice.DAL.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddDataAccessLayerServices(this IServiceCollection serviceCollection,
IConfiguration configuration)
    {

        return serviceCollection;
    }
}