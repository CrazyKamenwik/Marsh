using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using TicketSystem.API.BackgroundServices;
using TicketSystem.API.Extensions;
using TicketSystem.API.MapperProfiles;
using TicketSystem.API.Validators;
using TicketSystem.BLL.DI;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = configuration["Authentication:Domain"];
    options.Audience = configuration["Authentication:Audience"];
});

builder.Services
    .AddBusinessLogicLayerServices(configuration)
    .AddHostedService<TicketTimedHostedService>()
    .AddAutoMapper(typeof(MapperProfile).GetTypeInfo().Assembly)
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<ShortUserValidator>();

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGenWithAuthorization(configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API");
        c.OAuthClientId(configuration["Authentication:ClientId"]);
    });
}
app.UseAuthentication();
app.UseAuthorization();

app.ConfigureCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

public partial class Program { }