using CurrencyConversion.Application.DTOs;
using FluentValidation;

namespace CurrencyConversion.Application.Validators;

/// <summary>
/// Validates convert currency request.
/// </summary>
public class ConvertRequestValidator : AbstractValidator<ConvertRequest>
{
    public ConvertRequestValidator()
    {
        RuleFor(x => x.FromCurrency)
            .NotEmpty().WithMessage("From currency is required.")
            .Length(3).WithMessage("From currency must be a 3-letter ISO code.")
            .MustNotBeBlockedCurrency();

        RuleFor(x => x.ToCurrency)
            .NotEmpty().WithMessage("To currency is required.")
            .Length(3).WithMessage("To currency must be a 3-letter ISO code.")
            .MustNotBeBlockedCurrency();

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}
