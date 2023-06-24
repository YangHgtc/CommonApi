using CommonApi.DTO.Requests;
using FluentValidation;

namespace CommonApi.Validation;

public sealed class WeatherValidator : AbstractValidator<WeatherRequest>
{
    public WeatherValidator()
    {
        RuleFor(x => x.MaxTemp)
            .NotEmpty()
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(100);
    }
}
