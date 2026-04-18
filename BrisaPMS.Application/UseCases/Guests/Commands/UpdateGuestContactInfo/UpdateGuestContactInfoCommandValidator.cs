using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestContactInfo;

public class UpdateGuestContactInfoCommandValidator : AbstractValidator<UpdateGuestContactInfoCommand>
{
  public UpdateGuestContactInfoCommandValidator()
  {
    RuleFor(x => x.GuestId)
      .NotEmpty().WithMessage("The field GuestId is required.");
    
    RuleFor(x => x.Email)
      .NotEmpty().WithMessage("The field Email is required.")
      .MaximumLength(254).WithMessage("The field Email can't exceed 255 characters.")
      .EmailAddress().WithMessage("Must be a valid email address.");
    
    RuleFor(x => x.PhoneNumber)
      .NotEmpty().WithMessage("The field Phone Number is required.")
      .MaximumLength(25).WithMessage("The field Phone Number can't exceed 25 characters.")
      .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Must be a valid phone number");
  }
}