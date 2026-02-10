using System.Globalization;
using CurrencyConversion.Application.DTOs;
using CurrencyConversion.Application.Interfaces;
using CurrencyConversion.Infrastructure.Frankfurter;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace CurrencyConversion.Infrastructure.Providers;

/// <summary>
/// Currency rate provider that uses the Frankfurter API with in-memory caching.
/// </summary>
public sealed class FrankfurterCurrencyRateProvider : ICurrencyRateProvider
{
    public const string ProviderName = "Frankfurter";

    private readonly IFrankfurterApiClient _apiClient;
    private readonly IMemoryCache _cache;
    private readonly FrankfurterOptions _options;

    public FrankfurterCurrencyRateProvider(
        IFrankfurterApiClient apiClient,
        IMemoryCache cache,
        IOptions<FrankfurterOptions> options)
    {
        _apiClient = apiClient;
        _cache = cache;
        _options = options.Value;
    }

    public string Name => ProviderName;

    public async Task<LatestRatesResponse> GetLatestRatesAsync(string baseCurrency, CancellationToken cancellationToken = default)
    {
        var key = $"frankfurter:latest:{baseCurrency.ToUpperInvariant()}";
        return (await _cache.GetOrCreateAsync(key, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.LatestRatesCacheDuration;
            var response = await _apiClient.GetLatestAsync(baseCurrency, null, cancellationToken);
            return MapLatest(response);
        }))!;
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

    public async Task<HistoricalRatesResponse> GetHistoricalRatesAsync(HistoricalRatesRequest request, CancellationToken cancellationToken = default)
    {
        var quote = request.QuoteCurrency ?? "USD";
        var cacheKey = $"frankfurter:series:{request.BaseCurrency}:{quote}:{request.FromDate:yyyy-MM-dd}:{request.ToDate:yyyy-MM-dd}";
        var series = await _cache.GetOrCreateAsync(cacheKey, async entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = _options.HistoricalRatesCacheDuration;
            var response = await _apiClient.GetTimeSeriesAsync(
                request.FromDate,
                request.ToDate,
                request.BaseCurrency,
                quote,
                cancellationToken);
            return MapTimeSeriesToItems(response, quote);
        });

        if (series == null)
            series = new List<HistoricalRateItem>();

        var totalCount = series.Count;
        var pageItems = series
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new HistoricalRatesResponse(
            request.BaseCurrency.ToUpperInvariant(),
            request.QuoteCurrency?.ToUpperInvariant(),
            request.FromDate,
            request.ToDate,
            pageItems,
            request.Page,
            request.PageSize,
            totalCount);
    }

    private static LatestRatesResponse MapLatest(FrankfurterLatestResponse response)
    {
        var date = DateTime.TryParse(response.Date, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d)
            ? d
            : DateTime.UtcNow.Date;
        var rates = new Dictionary<string, decimal>(response.Rates, StringComparer.OrdinalIgnoreCase);
        return new LatestRatesResponse(response.Base, date, rates);
    }

    private static List<HistoricalRateItem> MapTimeSeriesToItems(FrankfurterTimeSeriesResponse response, string quoteCurrency)
    {
        var items = new List<HistoricalRateItem>();
        foreach (var (dateStr, rates) in response.Rates)
        {
            if (!DateTime.TryParse(dateStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                continue;
            if (rates.TryGetValue(quoteCurrency, out var rate))
                items.Add(new HistoricalRateItem(date, quoteCurrency, rate));
        }
        return items.OrderBy(x => x.Date).ToList();
    }
}
