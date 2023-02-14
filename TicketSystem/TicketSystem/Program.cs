using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using TicketSystem.API.BackgroundServices;
using TicketSystem.API.Extensions;
using TicketSystem.API.MapperProfiles;
using TicketSystem.API.Validators;
using TicketSystem.BLL.DI;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services
    .AddBusinessLogicLayerServices(configuration)
    .AddHostedService<TicketTimedHostedService>()
    .AddAutoMapper(typeof(MapperProfile).GetTypeInfo().Assembly)
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<ShortUserValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ConfigureCustomExceptionMiddleware();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();