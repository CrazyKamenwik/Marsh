using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Newtonsoft.Json;

namespace TicketSystem.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    // TODO #1: Create exceptions with correct StatusCodes
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = contextFeature.Error.Message }));
                });
            });
        }
    }
}