using BrisaPMS.Domain.HouseKeeping;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.CreateHouseKeepingTask;

public class CreateHouseKeepingTaskCommandValidator : AbstractValidator<CreateHouseKeepingTaskCommand>
{
    public CreateHouseKeepingTaskCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("The field RoomId is required.");
        
        RuleFor(x => x.AssignedTo)
            .NotEmpty().WithMessage("The field AssignedTo is required.");
        
        RuleFor(x => x.AssignedBy)
            .NotEmpty().WithMessage("The field Assigned By is required.");
        
        RuleFor(x => x.HouseKeepingTaskType)
            .NotEmpty().WithMessage("The field HouseKeeping Task Type is required.")
            .Must(x => Enum.IsDefined(typeof(HouseKeepingTaskType), x))
            .WithMessage("HouseKeeping Task Type not supported.");
        
        RuleFor(x => x.TaskPriority)
            .NotEmpty().WithMessage("The field Task Priority is required.")
            .Must(x => Enum.IsDefined(typeof(TaskPriority), x))
            .WithMessage("Task Priority not supported.");
        
        RuleFor(x => x.ExpectedStartTime)
            .NotEmpty().WithMessage("The field Expected Start Time is required.")
            .LessThan(x => x.ExpectedEndTime)
            .WithMessage("Expected start time must be earlier than Expected end time.");
        
        RuleFor(x => x.ExpectedEndTime)
            .NotEmpty().WithMessage("The field Expected End Time is required.")
            .GreaterThan(x => x.ExpectedStartTime)
            .WithMessage("Expected end time must be later than Expected start time.")
            .NotEqual(x => x.ExpectedStartTime)
            .WithMessage("Expected start time and end time cannot be the same.");
        
        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("The field Notes can't exceed 500 characters.");
    }
}
