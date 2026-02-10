using CurrencyConversion.Application.DTOs;
using CurrencyConversion.Application.Interfaces;

namespace CurrencyConversion.Infrastructure.Services;

/// <summary>
/// Application rates service that delegates to the active currency rate provider from the factory.
/// </summary>
public sealed class RatesService : IRatesService
{
    private readonly ICurrencyRateProviderFactory _providerFactory;

    public RatesService(ICurrencyRateProviderFactory providerFactory)
    {
        _providerFactory = providerFactory;
    }

    public Task<LatestRatesResponse> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken = default)
        => _providerFactory.GetProvider().GetLatestRatesAsync(baseCurrency, cancellationToken);

    public Task<ConvertResponse> ConvertAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken cancellationToken = default)
        => _providerFactory.GetProvider().ConvertAsync(fromCurrency, toCurrency, amount, cancellationToken);

    public Task<HistoricalRatesResponse> GetHistoricalRatesAsync(HistoricalRatesRequest request, CancellationToken cancellationToken = default)
        => _providerFactory.GetProvider().GetHistoricalRatesAsync(request, cancellationToken);
}
