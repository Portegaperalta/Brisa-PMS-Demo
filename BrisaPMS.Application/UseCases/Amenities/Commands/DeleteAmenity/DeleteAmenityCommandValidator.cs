using FluentValidation;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.DeleteAmenity;

public class DeleteAmenityCommandValidator : AbstractValidator<DeleteAmenityCommand>
{
    public DeleteAmenityCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("The field AmenityId is required.");
    }
}