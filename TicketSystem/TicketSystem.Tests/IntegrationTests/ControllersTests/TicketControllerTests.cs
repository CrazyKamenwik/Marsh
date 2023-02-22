using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using TicketSystem.DAL;
using TicketSystem.Tests.Initialize;
using Newtonsoft.Json;
using Shouldly;
using TicketSystem.API.ViewModels.Messages;
using TicketSystem.API.ViewModels.Tickets;
using TicketSystem.API;
using System.Net;

namespace TicketSystem.Tests.IntegrationTests.ControllersTests
{
    public class TicketControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _httpClient;
        private const int OpenTicketId = 1;

        public TicketControllerTests(WebApplicationFactory<Program> factory)
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

            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllTickets_ReturnIEnumerableTicketViewModels()
        {
            // Act
            var response = await _httpClient.GetAsync("/api/ticket");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var ticketsViewModel = JsonConvert.DeserializeObject<IEnumerable<TicketViewModel>>(responseContent);
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
            var responseContent = await response.Content.ReadAsStringAsync();
            var ticketViewModel = JsonConvert.DeserializeObject<TicketViewModel>(responseContent);
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
}