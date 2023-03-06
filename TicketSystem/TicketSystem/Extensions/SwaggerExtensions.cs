using Microsoft.OpenApi.Models;
using TicketSystem.API.Middleware;
using TicketSystem.API.Swagger;

namespace TicketSystem.API.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerGenWithAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-JWT-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    Implicit = new OpenApiOAuthFlow
                    {
                        Scopes = new Dictionary<string, string>
                        {
                            { "openid", "Open Id" }
                        },
                        AuthorizationUrl = new Uri(configuration["Authentication:Domain"] + "authorize?audience=" + configuration["Authentication:Audience"])
                    }
                }
            });

            c.OperationFilter<SecurityRequirementsOperationFilter>();
        });
    }
}