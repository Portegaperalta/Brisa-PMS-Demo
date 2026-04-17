using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Amenities.Commands.UpdateAmenityDetails;

public class UpdateAmenityDetailsCommandValidator : AbstractValidator<UpdateAmenityDetailsCommand>
{
    public UpdateAmenityDetailsCommandValidator()
    {
        RuleFor(x => x.AmenityId)
            .NotEmpty().WithMessage("The field AmenityId is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The field Name is required.")
            .MaximumLength(100).WithMessage("The field Name can't exceed 100 characters.");
        
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("The field Description is required.")
            .MaximumLength(500).WithMessage("The field Description can't exceed 500 characters.");
    }
}