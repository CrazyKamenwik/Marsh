using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketSystem.DAL;
using TicketSystem.Tests.Initialize;

namespace TicketSystem.Tests.IntegrationTests.WebAppFactory;

public class TestHttpClientFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationContext>));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("Data Source=MyDb.db"));

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationContext>();

                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                InitializeDb.Initialize(db);
            }
        });
    }
}