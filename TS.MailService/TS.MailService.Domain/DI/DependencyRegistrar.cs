using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TS.MailService.Domain.Abstraction.Services;
using TS.MailService.Domain.MapperProfiles;
using TS.MailService.Domain.Services;
using TS.MailService.Infrastructure.DI;

namespace TS.MailService.Domain.DI;

public static class DependencyRegistrar
{
    public static IServiceCollection AddDomainLayerServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddInfrastructureLayerServices(configuration);

        services.AddAutoMapper(typeof(MapperProfile).GetTypeInfo().Assembly);

        services.AddTransient<IEmailService, EmailService>();

        return services;
    }
}