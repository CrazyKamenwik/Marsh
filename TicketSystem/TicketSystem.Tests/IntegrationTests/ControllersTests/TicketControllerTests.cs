using FluentAssertions;
using Shouldly;
using TicketSystem.API.ViewModels.Tickets;
using System.Net;
using System.Net.Http.Json;
using TicketSystem.Tests.IntegrationTests.WebAppFactory;
using System.Net.Http.Headers;

namespace TicketSystem.Tests.IntegrationTests.ControllersTests;

public class TicketControllerTests : IClassFixture<TestHttpClientFactory<Program>>
{
    private readonly HttpClient _httpClient;
    private const int OpenTicketId = 1;

    public TicketControllerTests(TestHttpClientFactory<Program> factory)
    {
        _httpClient = factory.CreateClient();

        var accessToken = AutorizationForTests.GetAccessToken().GetAwaiter().GetResult();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    [Fact]
    public async Task GetAllTickets_ReturnIEnumerableTicketViewModels()
    {
        // Act
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
        var response = await _httpClient.GetAsync($"/api/ticket/{int.MaxValue}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}