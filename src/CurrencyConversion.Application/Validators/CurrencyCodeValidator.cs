using CurrencyConversion.Domain.Constants;
using FluentValidation;

namespace CurrencyConversion.Application.Validators;

/// <summary>
/// Shared rule: currency code must not be a blocked currency (TRY, PLN, THB, MXN).
/// </summary>
public static class CurrencyCodeValidator
{
    public static IRuleBuilderOptions<T, string> MustNotBeBlockedCurrency<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .Must(code => !BlockedCurrencies.IsBlocked(code))
            .WithMessage("Currency '{PropertyValue}' is not supported. Blocked currencies: TRY, PLN, THB, MXN.");
    }
}
