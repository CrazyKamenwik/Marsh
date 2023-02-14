using FluentValidation;
using TicketSystem.BackgroundServices;
using TicketSystem.BLL.DI;
using TicketSystem.Extensions;
using TicketSystem.Validators;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services
    .AddBusinessLogicLayerServices(configuration)
    .AddHostedService<TicketTimedHostedService>()
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