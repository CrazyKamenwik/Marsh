using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;

namespace TS.MailService.Tests.IntegrationTests;

public class TestHttpClientFactory<TProgram> : WebApplicationFactory<TProgram>
    where TProgram : class
{
    internal static MongoDbRunner _runner;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _runner = MongoDbRunner.Start(singleNodeReplSet: false);

        builder.ConfigureServices(services =>
        {
            var dbContextDescriptor =
                services.SingleOrDefault(d => d.ServiceType == typeof(IMongoClient));

            if (dbContextDescriptor != null) services.Remove(dbContextDescriptor);

            services.AddSingleton(new MongoClient(_runner.ConnectionString));
        });
    }
}