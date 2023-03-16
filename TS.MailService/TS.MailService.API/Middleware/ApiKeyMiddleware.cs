using System.Net;
using SendGrid.Helpers.Errors.Model;

namespace TS.MailService.Application.Middleware;

public class ApiKeyMiddleware
{
    private const string ApiKeyHeaderName = "x-api-key";
    private const string ForbiddenMessage = $"Access denied: no {ApiKeyHeaderName} header.";
    private const string UnauthorizedMessage = $"Access denied: no {ApiKeyHeaderName} header.";
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApiKeyMiddleware> _logger;

    private readonly RequestDelegate _next;

    /// <summary>
    ///     Initializes a new instance of the <see cref="ApiKeyMiddleware" /> class.
    /// </summary>
    /// <param name="configuration">IConfiguration.</param>
    /// <param name="next">Next step to provide request.</param>
    /// <param name="logger">Logger.</param>
    public ApiKeyMiddleware(RequestDelegate next, ILogger<ApiKeyMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    ///     Provides handler for check API key.
    /// </summary>
    /// <param name="context">Context from request.</param>
    /// <returns>A <see cref="Task" /> representing the result of the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Headers.Keys.Contains(ApiKeyHeaderName))
        {
            if (context.Request.Headers[ApiKeyHeaderName] == _configuration[ApiKeyHeaderName])
            {
                await _next.Invoke(context);
            }
            else
            {
                _logger.LogWarning(UnauthorizedMessage);
                throw new UnauthorizedAccessException(UnauthorizedMessage);
            }
        }
        else
        {
            _logger.LogWarning(ForbiddenMessage);
            throw new ForbiddenException(ForbiddenMessage);
        }
    }
}