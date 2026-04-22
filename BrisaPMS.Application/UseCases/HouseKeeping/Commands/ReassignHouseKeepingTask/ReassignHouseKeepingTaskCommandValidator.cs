using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.ReassignHouseKeepingTask;

public class ReassignHouseKeepingTaskCommandValidator : AbstractValidator<ReassignHouseKeepingTaskCommand>
{
    public ReassignHouseKeepingTaskCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
        
        RuleFor(x => x.AssignedTo)
            .NotEmpty().WithMessage("The field Assigned To is required.");
    }
}