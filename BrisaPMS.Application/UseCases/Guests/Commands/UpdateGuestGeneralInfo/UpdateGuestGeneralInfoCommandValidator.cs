using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;

public class UpdateGuestGeneralInfoCommandValidator : AbstractValidator<UpdateGuestGeneralInfoCommand>
{
    public UpdateGuestGeneralInfoCommandValidator()
    {
        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("The field First Name is required.")
            .MaximumLength(250).WithMessage("The field First Name can't exceed 250 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("The field LastName is required.")
            .MaximumLength(250).WithMessage("The field LastName can't exceed 250 characters.");
        
        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("The field Country can't exceed 100 characters.");
        
        RuleFor(x => x.PreferredLanguage)
            .MaximumLength(50).WithMessage("The field PreferredLanguage can't exceed 50 characters.");
    }
}