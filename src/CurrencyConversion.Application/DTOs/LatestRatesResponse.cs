namespace CurrencyConversion.Application.DTOs;

/// <summary>
/// Response for the latest exchange rates endpoint.
/// </summary>
public record LatestRatesResponse(
    string BaseCurrency,
    DateTime Date,
    IReadOnlyDictionary<string, decimal> Rates);
