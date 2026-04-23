using FluentValidation;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Commands.UpdateIncidentDescription;

public class UpdateIncidentDescriptionCommandValidator : AbstractValidator<UpdateIncidentDescriptionCommand>
{
    public UpdateIncidentDescriptionCommandValidator()
    {
        RuleFor(x => x.HouseKeepingTaskId)
            .NotEmpty().WithMessage("The field HouseKeepingTaskId is required.");
        
        RuleFor(x => x.IncidentDescription)
            .NotEmpty().WithMessage("The field Incident Description is required.")
            .MaximumLength(500).WithMessage("The field Incident Description can't exceed 500 characters.");
    }
}