using BrisaPMS.Domain.HouseKeeping;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskPriority;

public class UpdateHouseKeepingTaskPriorityCommandValidator : AbstractValidator<UpdateHouseKeepingTaskPriorityCommand>
{
    public UpdateHouseKeepingTaskPriorityCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
        
        RuleFor(x => x.TaskPriority)
            .NotEmpty().WithMessage("The field Task Priority is required.")
            .Must(x => Enum.IsDefined(typeof(TaskPriority), x))
            .WithMessage("Task priority not supported.");
    }
}