using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using TicketSystem.Middleware;

namespace TicketSystem.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}