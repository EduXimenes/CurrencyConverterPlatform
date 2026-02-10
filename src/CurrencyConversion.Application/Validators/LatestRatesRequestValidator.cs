using FluentValidation;

namespace CurrencyConversion.Application.Validators;

/// <summary>
/// Validates request for latest rates (e.g. base currency from query).
/// </summary>
public class LatestRatesRequestValidator : AbstractValidator<string>
{
    public LatestRatesRequestValidator()
    {
        RuleFor(x => x)
            .NotEmpty().WithMessage("Base currency is required.")
            .Length(3).WithMessage("Base currency must be a 3-letter ISO code.")
            .MustNotBeBlockedCurrency();
    }
}
