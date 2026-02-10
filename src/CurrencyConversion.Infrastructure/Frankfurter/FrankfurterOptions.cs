namespace CurrencyConversion.Infrastructure.Frankfurter;

/// <summary>
/// Configuration for the Frankfurter API client.
/// </summary>
public sealed class FrankfurterOptions
{
    public const string SectionName = "Frankfurter";

    /// <summary>
    /// Base URL for the Frankfurter API (e.g. https://api.frankfurter.dev).
    /// </summary>
    public string BaseAddress { get; set; } = "https://api.frankfurter.dev";

    /// <summary>
    /// Cache duration for latest rates (e.g. 1 hour). Default 00:30:00.
    /// </summary>
    public TimeSpan LatestRatesCacheDuration { get; set; } = TimeSpan.FromMinutes(30);

    /// <summary>
    /// Cache duration for historical/time series data. Default 24 hours.
    /// </summary>
    public TimeSpan HistoricalRatesCacheDuration { get; set; } = TimeSpan.FromHours(24);

    /// <summary>
    /// Number of retries for transient failures. Default 3.
    /// </summary>
    public int RetryCount { get; set; } = 3;

    /// <summary>
    /// Number of failures before opening the circuit. Default 5.
    /// </summary>
    public int CircuitBreakerFailureThreshold { get; set; } = 5;

    /// <summary>
    /// Duration the circuit stays open. Default 30 seconds.
    /// </summary>
    public TimeSpan CircuitBreakerDurationOfBreak { get; set; } = TimeSpan.FromSeconds(30);
}
