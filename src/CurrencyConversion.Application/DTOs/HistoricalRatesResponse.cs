namespace CurrencyConversion.Application.DTOs;

/// <summary>
/// Paginated response for historical rates.
/// </summary>
public record HistoricalRatesResponse(
    string BaseCurrency,
    string? QuoteCurrency,
    DateTime FromDate,
    DateTime ToDate,
    IReadOnlyList<HistoricalRateItem> Items,
    int Page,
    int PageSize,
    int TotalCount);

/// <summary>
/// A single historical rate entry.
/// </summary>
public record HistoricalRateItem(
    DateTime Date,
    string QuoteCurrency,
    decimal Rate);
