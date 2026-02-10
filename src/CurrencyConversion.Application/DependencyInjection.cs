using CurrencyConversion.Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConversion.Application;

/// <summary>
/// Registers Application layer services and validators.
/// </summary>
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<ConvertRequestValidator>();
        return services;
    }
}
