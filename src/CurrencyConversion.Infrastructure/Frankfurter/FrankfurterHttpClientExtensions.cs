using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace CurrencyConversion.Infrastructure.Frankfurter;

/// <summary>
/// Configures the named HttpClient for Frankfurter with Polly retry, circuit breaker, and correlation ID.
/// </summary>
public static class FrankfurterHttpClientExtensions
{
    public const string ClientName = "Frankfurter";

    public static IHttpClientBuilder AddFrankfurterHttpClient(this IServiceCollection services)
    {
        return services
            .AddHttpClient<FrankfurterApiClient>(ClientName, (sp, client) =>
            {
                var options = sp.GetRequiredService<IOptions<FrankfurterOptions>>().Value;
                client.BaseAddress = new Uri(options.BaseAddress.TrimEnd('/') + "/");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json");
            })
            .AddHttpMessageHandler<Http.CorrelationIdDelegatingHandler>()
            .AddPolicyHandler((sp, _) =>
            {
                var options = sp.GetRequiredService<IOptions<FrankfurterOptions>>().Value;
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        options.RetryCount,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            })
            .AddPolicyHandler((sp, _) =>
            {
                var options = sp.GetRequiredService<IOptions<FrankfurterOptions>>().Value;
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        options.CircuitBreakerFailureThreshold,
                        options.CircuitBreakerDurationOfBreak);
            });
    }
}
