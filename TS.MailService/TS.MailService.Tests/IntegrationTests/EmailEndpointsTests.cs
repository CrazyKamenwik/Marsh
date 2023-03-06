using FluentAssertions;
using Mongo2Go;
using System.Net.Http.Json;
using System.Net;
using TS.MailService.Application.Models;
using TS.MailService.Infrastructure.Entities;
using TS.MailService.Tests.InitializeModels;

namespace TS.MailService.Tests.IntegrationTests;

public class EmailEndpointsTests : IClassFixture<TestHttpClientFactory<Program>>
{
    private const string ApiKey = "59E0492F-7CC0-43D8-B262-60367970915C";
    private const string ApiKeyName = "x-api-key";

    private readonly HttpClient _httpClient;

    public EmailEndpointsTests(TestHttpClientFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        _httpClient.DefaultRequestHeaders.Add(ApiKeyName, ApiKey);
    }

    [Fact]
    public async Task Post_ValidMessage_ReturnsMessageViewModel()
    {
        // Arrange
        var email = InitializeData.GetShortEmailMessageFromUser();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/email", email);

        // Assert
        response.Should().BeSuccessful();
    }

    [Fact]
    public async Task Post_InvalidMessage_ReturnsBadRequest()
    {
        // Arrange
        var email = new ShortEmailMessageDto();

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/email", email);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetAllEmails_ReturnIEnumerableEmailMessageDTO()
    {
        // Act
        var response = await _httpClient.GetAsync("/api/email");

        // Assert
        response.EnsureSuccessStatusCode();
        var emails = await response.Content.ReadFromJsonAsync<IEnumerable<EmailMessageDto>>();
        emails.Should().NotBeNull();
        emails.Should().NotContainNulls(x => x.Body);
        emails.Should().NotContainNulls(x => x.Recipients);
        emails.Should().NotContainNulls(x => x.Subject);
    }

    [Fact]
    public async Task GetUserById_CorrectId_ReturnUserViewModel()
    {
        // Act
        var responseone = await _httpClient.GetAsync("/api/email");
        var emails = await responseone.Content.ReadFromJsonAsync<IEnumerable<EmailMessageDto>>();

        var response = await _httpClient.GetAsync($"/api/email/{InitializeData.UserEmailGuidForGetByIdTest}");

        // Assert
        response.EnsureSuccessStatusCode();
        var emailMessageDto = await response.Content.ReadFromJsonAsync<EmailMessageEntity>();

        emailMessageDto.Should().NotBeNull();
        emailMessageDto.Body.Should().NotBeNull();
        emailMessageDto.Recipients.Should().NotContainNulls();
        emailMessageDto.Subject.Should().NotBeNull();
    }

    [Fact]
    public async Task GetUserById_IncorrectId_ReturnNoContent()
    {
        // Act
        var response = await _httpClient.GetAsync($"/api/user/{Guid.NewGuid()}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}