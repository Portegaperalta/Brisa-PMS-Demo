using BrisaPMS.Domain.HouseKeeping;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeHouseKeepingTaskType;

public class ChangeHouseKeepingTaskTypeCommandValidator : AbstractValidator<ChangeHouseKeepingTaskTypeCommand>
{
    public ChangeHouseKeepingTaskTypeCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
        
        RuleFor(x => x.HouseKeepingTaskType)
            .NotEmpty().WithMessage("The field Task Type is required.")
            .Must(x => Enum.IsDefined(typeof(HouseKeepingTaskType), x))
            .WithMessage("HouseKeeping task type not supported");
    }
}