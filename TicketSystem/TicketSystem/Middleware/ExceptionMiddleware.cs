using System.Net;
using TicketSystem.API.ViewModels.Exceptions;

namespace TicketSystem.API.Middleware;

public class ExceptionMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong: {ex}", ex);
            await HandleException(httpContext, ex);
        }
    }

    private static async Task HandleException(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = exception switch
        {
            ArgumentException => (int)HttpStatusCode.BadRequest,
            _ => (int)HttpStatusCode.InternalServerError
        };

        await context.Response.WriteAsync(new ErrorDetails
        {
            StatusCode = context.Response.StatusCode,
            Message = exception.Message
        }.ToString());
    }
}