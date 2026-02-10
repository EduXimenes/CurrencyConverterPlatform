using CurrencyConversion.Application.DTOs;
using FluentValidation;

namespace CurrencyConversion.Application.Validators;

/// <summary>
/// Validates historical rates request.
/// </summary>
public class HistoricalRatesRequestValidator : AbstractValidator<HistoricalRatesRequest>
{
    public HistoricalRatesRequestValidator()
    {
        RuleFor(x => x.BaseCurrency)
            .NotEmpty().WithMessage("Base currency is required.")
            .Length(3).WithMessage("Base currency must be a 3-letter ISO code.")
            .MustNotBeBlockedCurrency();

        RuleFor(x => x.QuoteCurrency)
            .Length(3).WithMessage("Quote currency must be a 3-letter ISO code when provided.")
            .MustNotBeBlockedCurrency()
            .When(x => !string.IsNullOrWhiteSpace(x.QuoteCurrency));

        RuleFor(x => x.FromDate)
            .LessThanOrEqualTo(x => x.ToDate).WithMessage("From date must be before or equal to To date.");

        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be at least 1.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
    }
}
