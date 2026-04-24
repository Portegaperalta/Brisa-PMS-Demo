using FluentValidation;

namespace BrisaPMS.Application.UseCases.Stays.Commands.IncreaseNightCount;

public class IncreaseNightCountCommandValidator : AbstractValidator<IncreaseNightCountCommand>
{
    public IncreaseNightCountCommandValidator()
    {
        RuleFor(x => x.StayId)
            .NotEmpty().WithMessage("The field StayId is required.");
    }
}