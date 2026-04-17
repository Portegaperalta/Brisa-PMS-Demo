using FluentValidation;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.DeactivateAmenity;

public class DeactivateAmenityCommandValidator : AbstractValidator<DeactivateAmenityCommand>
{
    public DeactivateAmenityCommandValidator()
    {
        RuleFor(x => x.AmenityId)
            .NotEmpty().WithMessage("The field AmenityId is required.");
    }
}