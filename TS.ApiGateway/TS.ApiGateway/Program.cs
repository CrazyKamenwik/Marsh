using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using TS.ApiGateway;

var builder = WebApplication.CreateBuilder(args);
var routes = "Routes";

builder.Configuration.AddOcelotWithSwaggerSupport(option => option.Folder = routes);

builder.Services.AddOcelot(builder.Configuration).AddPolly();
builder.Services.AddSwaggerForOcelot(builder.Configuration);

builder.Configuration.AddOcelot(routes, builder.Environment)
    .AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseHttpsRedirection();

app.UseAuthorization();

var aasds = builder.Configuration["ClientId"];

app.UseSwaggerForOcelotUI(options =>
{
    options.PathToSwaggerGenerator = "/swagger/docs";
    options.ReConfigureUpstreamSwaggerJson = AlterUpstream.AlterUpstreamSwaggerJson;
}).UseOcelot().Wait();

app.MapControllers();

app.Run();