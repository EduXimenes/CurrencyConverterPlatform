namespace CurrencyConversion.Domain.Entities;

/// <summary>
/// Represents a single exchange rate at a point in time.
/// </summary>
public class ExchangeRate
{
    public string BaseCurrency { get; init; } = string.Empty;
    public string QuoteCurrency { get; init; } = string.Empty;
    public decimal Rate { get; init; }
    public DateTime Date { get; init; }
}
