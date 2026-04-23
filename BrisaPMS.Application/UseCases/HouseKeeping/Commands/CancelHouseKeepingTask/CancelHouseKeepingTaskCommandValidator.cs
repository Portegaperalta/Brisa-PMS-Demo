using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CancelHouseKeepingTask;

public class CancelHouseKeepingTaskCommandValidator : AbstractValidator<CancelHouseKeepingTaskCommand>
{
    public CancelHouseKeepingTaskCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
    }
}