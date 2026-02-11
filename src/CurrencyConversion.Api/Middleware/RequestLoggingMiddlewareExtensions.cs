using Microsoft.AspNetCore.Builder;

namespace CurrencyConversion.Api.Middleware;

/// <summary>
/// Extension to register the request logging middleware.
/// </summary>
public static class RequestLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app)
        => app.UseMiddleware<RequestLoggingMiddleware>();
}
