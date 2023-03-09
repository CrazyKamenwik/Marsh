using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Shouldly;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.Tests.IntegrationTests.WebAppFactory;

namespace TicketSystem.Tests.IntegrationTests.ControllersTests;

public class MessageControllerIntegrationTests : IClassFixture<TestHttpClientFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private const int UserId = 1;
    private const int OperatorId = 2;
    private const int OpenTicketId = 1;

    public MessageControllerIntegrationTests(TestHttpClientFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Post_ValidMessage_ReturnsMessageViewModel()
    {
        // Arrange
        var accessToken = await AutorizationForTests.GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var message = new ShortMessageViewModel { Text = "Hello", UserId = OperatorId, TicketId = OpenTicketId };

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
        var accessToken = await AutorizationForTests.GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var message = new ShortMessageViewModel { Text = text, UserId = userId, TicketId = ticketId };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/message", message);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }
}