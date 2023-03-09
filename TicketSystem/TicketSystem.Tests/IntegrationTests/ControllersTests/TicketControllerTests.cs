using FluentAssertions;
using Shouldly;
using TicketSystem.API.ViewModels.Tickets;
using System.Net;
using System.Net.Http.Json;
using TicketSystem.Tests.IntegrationTests.WebAppFactory;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;

namespace TicketSystem.Tests.IntegrationTests.ControllersTests;

public class TicketControllerTests : IClassFixture<TestHttpClientFactory<Program>>
{
    private const string GrantType = "client_credentials";

    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private const int OpenTicketId = 1;

    public TicketControllerTests(TestHttpClientFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();
    }

    private async Task<string> GetAccessToken()
    {
        var httpClient = new HttpClient();

        var tokenResponse = await httpClient.PostAsync(
            _config["Authentication:Domain"] + "oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", GrantType },
                { "client_id", _config["Authentication:ClientId"]! },
                { "client_secret", _config["Authentication:ClientSecret"]! },
                { "audience", _config["Authentication:Audience"]! }
            }));

        tokenResponse.EnsureSuccessStatusCode();

        var tokenData = await tokenResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        var accessToken = tokenData?["access_token"].ToString() ?? throw new ArgumentNullException(nameof(tokenData));

        return accessToken;
    }

    [Fact]
    public async Task GetAllTickets_ReturnIEnumerableTicketViewModels()
    {
        // Act
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync("/api/ticket");

        // Assert
        response.EnsureSuccessStatusCode();
        var ticketsViewModel = await response.Content.ReadFromJsonAsync<IEnumerable<TicketViewModel>>();
        ticketsViewModel.Should().NotContainNulls(x => x.Messages);
        ticketsViewModel.Should().NotContainNulls(x => x.TicketCreator);
    }

    [Fact]
    public async Task GetTicketById_CorrectId_ReturnTicketViewModels()
    {
        // Act
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync($"/api/ticket/{OpenTicketId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var ticketViewModel = await response.Content.ReadFromJsonAsync<TicketViewModel>();
        ticketViewModel.ShouldNotBeNull();
        ticketViewModel.TicketCreator.Should().NotBeNull();
        ticketViewModel.Messages.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetTicketById_IncorrectId_ReturnTicketViewModels()
    {
        // Act
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync($"/api/ticket/{int.MaxValue}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}