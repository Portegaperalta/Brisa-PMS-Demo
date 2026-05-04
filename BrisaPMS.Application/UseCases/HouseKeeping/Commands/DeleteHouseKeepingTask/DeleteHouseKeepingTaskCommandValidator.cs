using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.DeleteHouseKeepingTask;

public class DeleteHouseKeepingTaskCommandValidator : AbstractValidator<DeleteHouseKeepingTaskCommand>
{
    public DeleteHouseKeepingTaskCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
    }
}