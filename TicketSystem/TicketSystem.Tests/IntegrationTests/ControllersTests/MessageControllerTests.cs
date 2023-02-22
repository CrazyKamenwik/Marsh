using System.Net;
using System.Net.Http.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Shouldly;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.DAL;
using TicketSystem.Tests.Initialize;
using TicketSystem.Tests.IntegrationTests.WebAppFactory;

namespace TicketSystem.Tests.IntegrationTests.ControllersTests;

public class MessageControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private const int UserId = 1;
    private const int OperatorId = 2;
    private const int OpenTicketId = 1;

    public MessageControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _httpClient = TestHttpClientFactory.CreateHttpClient(factory);
    }

    [Theory]
    [InlineData("Hello", UserId, null)]
    [InlineData("Hello", OperatorId, OpenTicketId)]
    public async Task Post_ValidMessage_ReturnsMessageViewModel(string text, int userId, int? ticketId)
    {
        // Arrange
        var message = new ShortMessageViewModel { Text = text, UserId = userId, TicketId = ticketId };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/message", message);

        // Assert
        response.EnsureSuccessStatusCode();
        var messageViewModel = await response.Content.ReadFromJsonAsync<MessageViewModel>();
        messageViewModel!.Text.ShouldBeEquivalentTo(message.Text);
    }

    [Theory]
    [InlineData("", UserId, null)]
    [InlineData(null, UserId, null)]
    [InlineData("Hello", 0, null)]
    public async Task Post_InvalidMessage_ReturnsBadRequest(string text, int userId, int? ticketId)
    {
        // Arrange
        var message = new ShortMessageViewModel { Text = text, UserId = userId, TicketId = ticketId };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/message", message);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}