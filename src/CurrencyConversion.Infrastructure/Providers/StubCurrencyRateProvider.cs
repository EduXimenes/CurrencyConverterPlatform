using CurrencyConversion.Application.DTOs;
using CurrencyConversion.Application.Interfaces;

namespace CurrencyConversion.Infrastructure.Providers;

/// <summary>
/// Stub provider for testing; no external API calls.
/// </summary>
public sealed class StubCurrencyRateProvider : ICurrencyRateProvider
{
    public const string ProviderName = "Stub";

    public string Name => ProviderName;

    public Task<LatestRatesResponse> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var date = DateTime.UtcNow.Date;
        var rates = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase)
        {
            ["USD"] = 1.08m,
            ["GBP"] = 0.85m,
            ["EUR"] = 1.00m,
            ["JPY"] = 159.50m
        };
        return Task.FromResult(new LatestRatesResponse(baseCurrency.ToUpperInvariant(), date, rates));
    }

    public async Task<ConvertResponse> ConvertAsync(string fromCurrency, string toCurrency, decimal amount, CancellationToken cancellationToken = default)
    {
        var latest = await GetLatestRatesAsync(fromCurrency, cancellationToken);
        var rate = latest.Rates.TryGetValue(toCurrency.ToUpperInvariant(), out var r) ? r : 1m;
        return new ConvertResponse(
            fromCurrency.ToUpperInvariant(),
            toCurrency.ToUpperInvariant(),
            amount,
            amount * rate,
            rate,
            latest.Date);
    }

    public Task<HistoricalRatesResponse> GetHistoricalRatesAsync(HistoricalRatesRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var quote = request.QuoteCurrency ?? "USD";
        var allDates = new List<DateTime>();
        for (var d = request.FromDate.Date; d <= request.ToDate.Date; d = d.AddDays(1))
            allDates.Add(d);
        var totalCount = allDates.Count;
        var pageItems = allDates
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(d => new HistoricalRateItem(d, quote, 1m + (decimal)(d.Ticks % 1000) / 10000m))
            .ToList();
        return Task.FromResult(new HistoricalRatesResponse(
            request.BaseCurrency.ToUpperInvariant(),
            request.QuoteCurrency?.ToUpperInvariant(),
            request.FromDate,
            request.ToDate,
            pageItems,
            request.Page,
            request.PageSize,
            totalCount));
    }
}
