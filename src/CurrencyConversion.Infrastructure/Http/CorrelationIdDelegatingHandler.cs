using Microsoft.AspNetCore.Http;

namespace CurrencyConversion.Infrastructure.Http;

/// <summary>
/// Propagates correlation ID from incoming request to outgoing Frankfurter API calls.
/// Reads X-Correlation-ID from request; if missing, generates a new one.
/// </summary>
public sealed class CorrelationIdDelegatingHandler : DelegatingHandler
{
    public const string CorrelationIdHeaderName = "X-Correlation-ID";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CorrelationIdDelegatingHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var correlationId = GetOrCreateCorrelationId();
        request.Headers.TryAddWithoutValidation(CorrelationIdHeaderName, correlationId);
        return await base.SendAsync(request, cancellationToken);
    }

    private string GetOrCreateCorrelationId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context?.Request.Headers.TryGetValue(CorrelationIdHeaderName, out var value) == true &&
            !string.IsNullOrWhiteSpace(value))
            return value!;

        return Guid.NewGuid().ToString("N");
    }
}
