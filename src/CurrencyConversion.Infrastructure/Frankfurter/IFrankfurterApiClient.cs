namespace CurrencyConversion.Infrastructure.Frankfurter;

/// <summary>
/// Low-level client for the Frankfurter API.
/// Used by <see cref="FrankfurterCurrencyRateProvider"/>; enables testing with a fake.
/// </summary>
public interface IFrankfurterApiClient
{
    Task<FrankfurterLatestResponse> GetLatestAsync(string baseCurrency, string? symbols = null, CancellationToken cancellationToken = default);

    Task<FrankfurterLatestResponse> GetHistoricalAsync(DateTime date, string baseCurrency, string? symbols = null, CancellationToken cancellationToken = default);

    Task<FrankfurterTimeSeriesResponse> GetTimeSeriesAsync(DateTime from, DateTime to, string baseCurrency, string? symbols = null, CancellationToken cancellationToken = default);
}
