using CurrencyConversion.Application.DTOs;

namespace CurrencyConversion.Application.Interfaces;

/// <summary>
/// Contract for a currency rate provider (e.g. Frankfurter, stub, or future providers).
/// Enables the factory pattern for multiple providers.
/// </summary>
public interface ICurrencyRateProvider
{
    string Name { get; }

    Task<LatestRatesResponse> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken = default);

    Task<ConvertResponse> ConvertAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken cancellationToken = default);

    Task<HistoricalRatesResponse> GetHistoricalRatesAsync(HistoricalRatesRequest request, CancellationToken cancellationToken = default);
}
