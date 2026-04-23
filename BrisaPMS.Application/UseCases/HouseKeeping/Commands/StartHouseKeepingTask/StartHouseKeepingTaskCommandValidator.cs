using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.StartHouseKeepingTask;

public class StartHouseKeepingTaskCommandValidator : AbstractValidator<StartHouseKeepingTaskCommand>
{
    public StartHouseKeepingTaskCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
    }
}