using FluentValidation;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.ActivateAmenity;

public class ActivateAmenityCommandValidator : AbstractValidator<ActivateAmenityCommand>
{
    public ActivateAmenityCommandValidator()
    {
        RuleFor(x => x.AmenityId)
            .NotEmpty().WithMessage("The field AmenityId is required.");
    }
}