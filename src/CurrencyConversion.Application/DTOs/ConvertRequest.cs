namespace CurrencyConversion.Application.DTOs;

/// <summary>
/// Request to convert an amount from one currency to another.
/// </summary>
public record ConvertRequest(
    string FromCurrency,
    string ToCurrency,
    decimal Amount);
