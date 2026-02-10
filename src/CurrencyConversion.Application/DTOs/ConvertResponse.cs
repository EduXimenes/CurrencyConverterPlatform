namespace CurrencyConversion.Application.DTOs;

/// <summary>
/// Response for a currency conversion.
/// </summary>
public record ConvertResponse(
    string FromCurrency,
    string ToCurrency,
    decimal Amount,
    decimal Result,
    decimal Rate,
    DateTime Date);
