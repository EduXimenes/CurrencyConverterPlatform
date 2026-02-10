using System.Globalization;
using System.Net.Http.Json;

namespace CurrencyConversion.Infrastructure.Frankfurter;

/// <summary>
/// HTTP client for the Frankfurter API (https://api.frankfurter.dev).
/// Uses HttpClient from IHttpClientFactory; Polly and correlation ID are configured in DI.
/// </summary>
public sealed class FrankfurterApiClient : IFrankfurterApiClient
{
    private readonly HttpClient _httpClient;

    public FrankfurterApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FrankfurterLatestResponse> GetLatestAsync(string baseCurrency, string? symbols = null, CancellationToken cancellationToken = default)
    {
        var query = $"base={Uri.EscapeDataString(baseCurrency)}";
        if (!string.IsNullOrWhiteSpace(symbols))
            query += $"&symbols={Uri.EscapeDataString(symbols)}";
        var response = await _httpClient.GetAsync($"v1/latest?{query}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<FrankfurterLatestResponse>(cancellationToken);
        return result ?? new FrankfurterLatestResponse();
    }

    public async Task<FrankfurterLatestResponse> GetHistoricalAsync(DateTime date, string baseCurrency, string? symbols = null, CancellationToken cancellationToken = default)
    {
        var dateStr = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var query = $"base={Uri.EscapeDataString(baseCurrency)}";
        if (!string.IsNullOrWhiteSpace(symbols))
            query += $"&symbols={Uri.EscapeDataString(symbols)}";
        var response = await _httpClient.GetAsync($"v1/{dateStr}?{query}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<FrankfurterLatestResponse>(cancellationToken);
        return result ?? new FrankfurterLatestResponse();
    }

    public async Task<FrankfurterTimeSeriesResponse> GetTimeSeriesAsync(DateTime from, DateTime to, string baseCurrency, string? symbols = null, CancellationToken cancellationToken = default)
    {
        var fromStr = from.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var toStr = to.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        var query = $"base={Uri.EscapeDataString(baseCurrency)}";
        if (!string.IsNullOrWhiteSpace(symbols))
            query += $"&symbols={Uri.EscapeDataString(symbols)}";
        var response = await _httpClient.GetAsync($"v1/{fromStr}..{toStr}?{query}", cancellationToken);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<FrankfurterTimeSeriesResponse>(cancellationToken);
        return result ?? new FrankfurterTimeSeriesResponse();
    }
}
