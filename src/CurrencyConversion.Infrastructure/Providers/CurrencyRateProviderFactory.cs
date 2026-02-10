using CurrencyConversion.Application.Interfaces;
using Microsoft.Extensions.Options;

namespace CurrencyConversion.Infrastructure.Providers;

/// <summary>
/// Factory that resolves the active currency rate provider by name.
/// Default provider is configurable (e.g. "Frankfurter" or "Stub").
/// </summary>
public sealed class CurrencyRateProviderFactory : ICurrencyRateProviderFactory
{
    private readonly IEnumerable<ICurrencyRateProvider> _providers;
    private readonly CurrencyRateProviderFactoryOptions _options;

    public CurrencyRateProviderFactory(
        IEnumerable<ICurrencyRateProvider> providers,
        IOptions<CurrencyRateProviderFactoryOptions> options)
    {
        _providers = providers;
        _options = options.Value;
    }

    public ICurrencyRateProvider GetProvider(string? name = null)
    {
        var key = string.IsNullOrWhiteSpace(name) ? _options.DefaultProviderName : name.Trim();
        var provider = _providers.FirstOrDefault(p => p.Name.Equals(key, StringComparison.OrdinalIgnoreCase))
            ?? _providers.FirstOrDefault(p => p.Name.Equals(_options.DefaultProviderName, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
            throw new InvalidOperationException($"Currency rate provider '{key}' is not registered.");

        return provider;
    }
}
