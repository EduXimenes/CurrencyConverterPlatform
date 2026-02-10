using CurrencyConversion.Application.DTOs;

namespace CurrencyConversion.Application.Interfaces;

/// <summary>
/// Contract for fetching and converting exchange rates.
/// Implemented by Infrastructure (no external API calls in this solution yet).
/// </summary>
public interface IRatesService
{
    /// <summary>
    /// Gets the latest exchange rates for a base currency.
    /// </summary>
    Task<LatestRatesResponse> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken = default);

    /// <summary>
    /// Converts an amount from one currency to another using latest rate.
    /// </summary>
    Task<ConvertResponse> ConvertAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets historical rates with pagination.
    /// </summary>
    Task<HistoricalRatesResponse> GetHistoricalRatesAsync(HistoricalRatesRequest request, CancellationToken cancellationToken = default);
}
