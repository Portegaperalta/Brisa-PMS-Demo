using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ChangeTaskDeadline;

public class ChangeTaskDeadlineCommandValidator : AbstractValidator<ChangeTaskDeadlineCommand>
{
    public ChangeTaskDeadlineCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
        
        RuleFor(x => x.ExpectedStartTime)
            .NotEmpty().WithMessage("The field Expected Start Time is required.")
            .LessThan(x => x.ExpectedEndTime)
            .WithMessage("Expected start time must be earlier than Expected end time.")
            .NotEqual(x => x.ExpectedStartTime)
            .WithMessage("Expected start time and end time cannot be the same.");
        
        RuleFor(x => x.ExpectedEndTime)
            .NotEmpty().WithMessage("The field Expected End Time is required.")
            .GreaterThan(x => x.ExpectedStartTime)
            .WithMessage("Expected end time must be later than Expected start time.");
    }
}