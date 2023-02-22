using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using Shouldly;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.DAL;
using TicketSystem.Tests.IntergationTests.InitializeModels;

namespace TicketSystem.Tests.IntegrationTests.ControllersTests;

public class MessageControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private const int UserId = 1;
    private const int OperatorId = 2;
    private const int OpenTicketId = 1;

    public MessageControllerIntegrationTests(WebApplicationFactory<Program> factory)
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
                    db.Database.EnsureDeleted();
                    db.Database.EnsureCreated();
                    InitializeDb.Initialize(db);
                }
            });
        });

        _httpClient = factory.CreateClient();
    }

    [Theory]
    [InlineData("Hi", UserId, null)]
    [InlineData("Hello", UserId, null)]
    [InlineData("Hello", OperatorId, OpenTicketId)]
    [InlineData("Hi", OperatorId, OpenTicketId)]
    public async Task Post_ValidMessage_ReturnsMessageViewModel(string text, int userId, int? ticketId)
    {
        // Arrange
        var message = new ShortMessageViewModel { Text = text, UserId = userId, TicketId = ticketId };
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/message", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var messageViewModel = JsonConvert.DeserializeObject<MessageViewModel>(responseContent);
        messageViewModel!.Text.ShouldBeEquivalentTo(message.Text);
    }

    [Theory]
    [InlineData("", UserId, null)]
    [InlineData(null, UserId, null)]
    [InlineData("Hello", 0, null)]
    [InlineData("Hello", int.MinValue, null)]
    public async Task Post_InvalidMessage_ReturnsBadRequest(string text, int userId, int? ticketId)
    {
        // Arrange
        var message = new ShortMessageViewModel { Text = text, UserId = userId, TicketId = ticketId };
        var content = new StringContent(JsonConvert.SerializeObject(message), Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/message", content);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}