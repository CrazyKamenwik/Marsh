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

internal static class AutorizationForTests
{
    private const string GrantType = "client_credentials";

    private static IConfiguration Init()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();

        return config;
    }

    public static async Task<string> GetAccessToken()
    {
        var config = Init();

        var httpClient = new HttpClient();

        var tokenResponse = await httpClient.PostAsync(
            config["Authentication:Domain"] + "oauth/token",
            new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "grant_type", GrantType },
                { "client_id", config["Authentication:ClientId"]! },
                { "client_secret", config["Authentication:ClientSecret"]! },
                { "audience", config["Authentication:Audience"]! }
            }));

        tokenResponse.EnsureSuccessStatusCode();

        var tokenData = await tokenResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        var accessToken = tokenData?["access_token"].ToString() ?? throw new ArgumentNullException(nameof(tokenData));

        return accessToken;
    }
}