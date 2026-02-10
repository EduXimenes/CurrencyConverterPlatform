using System.Text.Json.Serialization;

namespace CurrencyConversion.Infrastructure.Frankfurter;

/// <summary>
/// Frankfurter API: GET /v1/latest or /v1/{date}
/// </summary>
public sealed class FrankfurterLatestResponse
{
    [JsonPropertyName("base")]
    public string Base { get; init; } = string.Empty;

    [JsonPropertyName("date")]
    public string Date { get; init; } = string.Empty;

    [JsonPropertyName("rates")]
    public Dictionary<string, decimal> Rates { get; init; } = new();
}

/// <summary>
/// Frankfurter API: GET /v1/{start..end} time series
/// </summary>
public sealed class FrankfurterTimeSeriesResponse
{
    [JsonPropertyName("base")]
    public string Base { get; init; } = string.Empty;

    [JsonPropertyName("start_date")]
    public string StartDate { get; init; } = string.Empty;

    [JsonPropertyName("end_date")]
    public string EndDate { get; init; } = string.Empty;

    [JsonPropertyName("rates")]
    public Dictionary<string, Dictionary<string, decimal>> Rates { get; init; } = new();
}
