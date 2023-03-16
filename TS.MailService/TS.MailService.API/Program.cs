using FastEndpoints;
using FastEndpoints.Swagger;
using TS.MailService.Application.Middleware;
using TS.MailService.Application.SwaggerOperationProcessors;
using TS.MailService.Domain.DI;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

builder.Services.AddDomainLayerServices(configuration);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDoc(addJWTBearerAuth: false, settings: opt =>
    opt.OperationProcessors.Add(new AddRequiredHeaderParameter()));
builder.Services.AddFastEndpoints();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseFastEndpoints(opt =>
{
    opt.Endpoints.Configurator = ept => ept.AllowAnonymous();
});

app.UseSwaggerGen();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<ApiKeyMiddleware>();
app.MapControllers();

app.Run();

public partial class Program { }