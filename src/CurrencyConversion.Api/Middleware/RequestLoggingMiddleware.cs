using System.Diagnostics;
using System.Security.Claims;
using Serilog;

namespace CurrencyConversion.Api.Middleware;

/// <summary>
/// Logs each request with client IP, client ID (from JWT), endpoint, status code, and response time using Serilog structured logging.
/// </summary>
public sealed class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var path = context.Request.Path.Value ?? "-";
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "-";
        var clientId = GetClientId(context);

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var statusCode = context.Response.StatusCode;

            Log.Information(
                "Request completed {ClientIp} {ClientId} {Endpoint} {StatusCode} {ResponseTimeMs}ms",
                clientIp,
                clientId,
                path,
                statusCode,
                stopwatch.ElapsedMilliseconds);
        }
    }

    private static string GetClientId(HttpContext context)
    {
        var sub = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? context.User.FindFirst("sub")?.Value;
        return string.IsNullOrEmpty(sub) ? "anonymous" : sub;
    }
}
