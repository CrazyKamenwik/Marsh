﻿using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Shouldly;
using TicketSystem.API.ViewModels.Users;
using TicketSystem.Tests.IntegrationTests.WebAppFactory;

namespace TicketSystem.Tests.IntegrationTests.ControllersTests;

public class UserControllerTests : IClassFixture<TestHttpClientFactory<Program>>
{
    private const string GrantType = "client_credentials";

    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private const int UserId = 1;
    private const int UserIdForDelete = 3;

    public UserControllerTests(TestHttpClientFactory<Program> factory)
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

    [Theory]
    [InlineData("Vlad", "Admin")]
    [InlineData("Olha", "Operator")]
    [InlineData("Jo", "User")]
    public async Task Post_ValidUser_ReturnUserViewModel(string name, string userRole)
    {
        // Arrange
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var user = new ShortUserViewModel()
        {
            Name = name,
            UserRole = userRole
        };

        // Act
        var response = await _httpClient.PostAsJsonAsync("/api/user", user);

        // Assert
        response.EnsureSuccessStatusCode();
        var userViewModel = await response.Content.ReadFromJsonAsync<UserViewModel>();
        userViewModel!.Name.ShouldBe(user.Name);
        userViewModel!.UserRole.ShouldBe(user.UserRole);
    }

    [Theory]
    [InlineData("V", "Admin")]
    [InlineData(null, "Operator")]
    [InlineData("", "User")]
    [InlineData("Vlad", "")]
    [InlineData("Vlad", null)]
    public async Task Post_InvalidUser_ReturnBadRequest(string name, string userRole)
    {
        // Arrange
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

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

    [Fact]
    public async Task GetAllUsers_ReturnIEnumerableUserViewModels()
    {
        // Act
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync("/api/user");

        // Assert
        response.EnsureSuccessStatusCode();
        var userViewModel = await response.Content.ReadFromJsonAsync<IEnumerable<UserViewModel>>();
        userViewModel.Should().NotContainNulls(x => x.UserRole);
        userViewModel.Should().NotContainNulls(x => x.Name);
    }

    [Fact]
    public async Task GetUserById_CorrectId_ReturnUserViewModel()
    {
        // Act
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync($"/api/user/{UserId}");

        // Assert
        response.EnsureSuccessStatusCode();
        var userViewModel = await response.Content.ReadFromJsonAsync<UserViewModel>();

        userViewModel.ShouldNotBeNull();
        userViewModel.Id.ShouldNotBeInRange(int.MinValue, 0);
        userViewModel.Name.ShouldNotBeNullOrEmpty();
        userViewModel.UserRole.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetUserById_IncorrectId_ReturnNoContent()
    {
        // Act
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync($"/api/user/{int.MaxValue}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task Delete_CorrectId_ReturnUserViewModel()
    {
        // Act
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.DeleteAsync($"/api/user/{UserIdForDelete}");

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("X", "Admin")]
    [InlineData(null, "Operator")]
    [InlineData("", "User")]
    [InlineData("Vlados", "")]
    [InlineData("Vlad", null)]
    public async Task Put_InvalidUser_ReturnsBadRequest(string name, string userRole)
    {
        // Arrange
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var user = new ShortUserViewModel()
        {
            Name = name,
            UserRole = userRole
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/api/user/{UserId}", user);

        // Assert
        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    [Theory]
    [InlineData("Vlad", "Admin")]
    [InlineData("Olha", "Operator")]
    [InlineData("Jo", "User")]
    public async Task Put_ValidUser_ReturnUserViewModel(string name, string userRole)
    {
        // Arrange
        var accessToken = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var user = new ShortUserViewModel()
        {
            Name = name,
            UserRole = userRole
        };

        // Act
        var response = await _httpClient.PutAsJsonAsync($"/api/user/{UserId}", user);

        // Assert
        response.EnsureSuccessStatusCode();
        var userViewModel = await response.Content.ReadFromJsonAsync<UserViewModel>();
        userViewModel!.Name.ShouldBe(user.Name);
        userViewModel!.UserRole.ShouldBe(user.UserRole);
    }
}