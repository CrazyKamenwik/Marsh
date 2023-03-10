using Telegram.Bot.Requests;
using TS.Messengers.Microservice.API.Middlewares;
using TS.Messengers.Microservice.BLL.DI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBusinessLogicLayerServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();