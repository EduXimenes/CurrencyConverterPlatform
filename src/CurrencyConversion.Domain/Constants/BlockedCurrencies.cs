namespace CurrencyConversion.Domain.Constants;

/// <summary>
/// Currencies that are not supported by the API (return HTTP 400).
/// </summary>
public static class BlockedCurrencies
{
    public static readonly IReadOnlySet<string> Codes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        "TRY",
        "PLN",
        "THB",
        "MXN"
    };

    public static bool IsBlocked(string currencyCode) => Codes.Contains(currencyCode?.Trim() ?? string.Empty);
}
