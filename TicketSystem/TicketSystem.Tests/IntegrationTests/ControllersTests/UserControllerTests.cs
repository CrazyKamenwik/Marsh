using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.API.ViewModels.Users;
using TicketSystem.BLL.Models;
using TicketSystem.DAL;
using TicketSystem.Tests.IntergationTests.InitializeModels;

namespace TicketSystem.Tests.IntegrationTests.ControllersTests;

public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private const int UserId = 1;
    private const int OperatorId = 2;
    private const int OpenTicketId = 1;

    public UserControllerTests(WebApplicationFactory<Program> factory)
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
    [InlineData("Vlad", "Admin")]
    [InlineData("Olha", "Operator")]
    [InlineData("Jo", "User")]
    public async Task Post_ValidUser_ReturnUserViewModel(string name, string userRole)
    {
        // Arrange
        var user = new ShortUserViewModel()
        {
            Name = name,
            UserRole = userRole
        };

        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/user", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var responseContent = await response.Content.ReadAsStringAsync();
        var messageViewModel = JsonConvert.DeserializeObject<UserViewModel>(responseContent);
        messageViewModel!.Name.ShouldBe(user.Name);
        messageViewModel!.UserRole.ShouldBe(user.UserRole);
    }

    [Theory]
    [InlineData("V", "Admin")]
    [InlineData("Olha", "Operator")]
    [InlineData("Jo", "User")]
    public async Task Post_InvalidUser_ReturnsBadRequest(string name, string userRole)
    {
        // Arrange
        var user = new ShortUserViewModel()
        {
            Name = name,
            UserRole = userRole
        };

        var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/user", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}