using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

namespace TicketSystem.Tests.IntegrationTests.WebAppFactory;

internal class AutorizationForTests
{
    private const string GrantType = "client_credentials";
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;

    public AutorizationForTests()
    {
        _httpClient = new HttpClient();
        _config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();
    }

    public async Task<string> GetAccessToken()
    {
        var tokenResponse = await _httpClient.PostAsync(
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
}