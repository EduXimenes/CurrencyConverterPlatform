namespace CurrencyConversion.Application.DTOs;

/// <summary>
/// Request for historical rates with optional pagination.
/// </summary>
public record HistoricalRatesRequest(
    string BaseCurrency,
    string? QuoteCurrency,
    DateTime FromDate,
    DateTime ToDate,
    int Page = 1,
    int PageSize = 10);
