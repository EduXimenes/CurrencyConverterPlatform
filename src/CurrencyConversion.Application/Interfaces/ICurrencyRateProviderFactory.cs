namespace CurrencyConversion.Application.Interfaces;

/// <summary>
/// Factory for resolving currency rate providers by name.
/// Enables multiple providers (Frankfurter, stub, etc.) and configuration-driven selection.
/// </summary>
public interface ICurrencyRateProviderFactory
{
    /// <summary>
    /// Gets the provider with the given name, or the default provider if name is null/empty.
    /// </summary>
    ICurrencyRateProvider GetProvider(string? name = null);
}
