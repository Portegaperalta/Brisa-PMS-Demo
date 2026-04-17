using FluentValidation;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.CreateAmenity;

public class CreateAmenityCommandValidator : AbstractValidator<CreateAmenityCommand>
{
    public CreateAmenityCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The field Name is required.")
            .MaximumLength(100).WithMessage("The field Name can't exceed 100 characters.");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("The field Description is required.")
            .MaximumLength(500).WithMessage("The field Description can't exceed 500 characters.");
    }
}