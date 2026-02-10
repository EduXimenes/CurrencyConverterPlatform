namespace CurrencyConversion.Infrastructure.Providers;

/// <summary>
/// Configuration for the currency rate provider factory.
/// </summary>
public sealed class CurrencyRateProviderFactoryOptions
{
    public const string SectionName = "CurrencyRateProvider";

    /// <summary>
    /// Default provider name when none is specified (e.g. "Frankfurter" or "Stub").
    /// </summary>
    public string DefaultProviderName { get; set; } = "Frankfurter";
}
