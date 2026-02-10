using CurrencyConversion.Application.Interfaces;
using CurrencyConversion.Infrastructure.Frankfurter;
using CurrencyConversion.Infrastructure.Providers;
using CurrencyConversion.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConversion.Infrastructure;

/// <summary>
/// Registers Infrastructure: Frankfurter API client (HttpClientFactory, Polly, correlation ID),
/// in-memory caching, provider factory, and rates service.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers Infrastructure with the given configuration.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<FrankfurterOptions>(configuration.GetSection(FrankfurterOptions.SectionName));
        services.Configure<CurrencyRateProviderFactoryOptions>(configuration.GetSection(CurrencyRateProviderFactoryOptions.SectionName));

        services.AddMemoryCache();
        // IHttpContextAccessor must be registered by the host (e.g. ASP.NET Core) for correlation ID propagation

        services.AddTransient<Http.CorrelationIdDelegatingHandler>();
        services.AddFrankfurterHttpClient();
        // Resolve IFrankfurterApiClient from the typed client so it gets the named HttpClient with BaseAddress
        services.AddTransient<IFrankfurterApiClient>(sp => sp.GetRequiredService<FrankfurterApiClient>());

        services.AddSingleton<ICurrencyRateProvider, FrankfurterCurrencyRateProvider>();
        services.AddSingleton<ICurrencyRateProvider, StubCurrencyRateProvider>();
        services.AddSingleton<ICurrencyRateProviderFactory, CurrencyRateProviderFactory>();
        services.AddScoped<IRatesService, RatesService>();

        return services;
    }

    /// <summary>
    /// Registers Infrastructure with default options (no config binding).
    /// Use when configuration is not available (e.g. tests); options use default values.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        var config = new ConfigurationBuilder().Build();
        return services.AddInfrastructure(config);
    }
}
