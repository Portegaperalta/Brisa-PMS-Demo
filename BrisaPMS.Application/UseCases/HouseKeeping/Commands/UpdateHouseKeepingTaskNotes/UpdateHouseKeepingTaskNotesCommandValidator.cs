using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateHouseKeepingTaskNotes;

public class UpdateHouseKeepingTaskNotesCommandValidator : AbstractValidator<UpdateHouseKeepingTaskNotesCommand>
{
    public UpdateHouseKeepingTaskNotesCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
        
        RuleFor(x => x.Notes)
            .NotEmpty().WithMessage("The field Notes is required.")
            .MaximumLength(500).WithMessage("The field Notes can't exceed 500 characters.");
    }
}