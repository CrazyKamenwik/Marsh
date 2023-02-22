using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TicketSystem.DAL;
using TicketSystem.Tests.Initialize;

namespace TicketSystem.Tests.IntegrationTests.WebAppFactory;

public static class TestHttpClientFactory
{
    public static HttpClient CreateHttpClient(WebApplicationFactory<Program> factory)
    {
        factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                var inMemoryRoot = new InMemoryDatabaseRoot();
                services.AddSingleton(inMemoryRoot);
                services.AddDbContext<ApplicationContext>(optionsBuilder =>
                    optionsBuilder.UseInMemoryDatabase("MyDb", inMemoryRoot));

                using (var scoped = services.BuildServiceProvider().CreateScope())
                {
                    var db = scoped.ServiceProvider.GetRequiredService<ApplicationContext>();
                    InitializeDb.Initialize(db);
                }
            });
        });

        return factory.CreateClient();
    }
}