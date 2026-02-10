using CurrencyConversion.Application.Interfaces;
using CurrencyConversion.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConversion.Infrastructure;

/// <summary>
/// Registers Infrastructure implementations (stubs; no external API yet).
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IRatesService, StubRatesService>();
        return services;
    }
}
