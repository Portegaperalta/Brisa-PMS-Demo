using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CompleteHouseKeepingTask;

public class CompleteHouseKeepingTaskCommandValidator : AbstractValidator<CompleteHouseKeepingTaskCommand>
{
    public CompleteHouseKeepingTaskCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
    }
}