using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelContactInfo;

public class UpdateHotelContactInfoValidator : AbstractValidator<UpdateHotelContactInfoCommand>
{
    public UpdateHotelContactInfoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Field Id is required.");
        
        RuleFor(x => x.BusinessEmail)
            .NotEmpty().WithMessage("Field Business Email is required.")
            .EmailAddress().WithMessage("Must be a valid email address")
            .MaximumLength(254).WithMessage("The field Business Email can't exceed 254 characters");
        
        RuleFor(x => x.BusinessPhoneNumber)
            .NotEmpty().WithMessage("Field Business Phone Number is required")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Must be a valid phone number")
            .MaximumLength(25).WithMessage("The field Business Phone Number can't exceed 25 characters");
    }
}