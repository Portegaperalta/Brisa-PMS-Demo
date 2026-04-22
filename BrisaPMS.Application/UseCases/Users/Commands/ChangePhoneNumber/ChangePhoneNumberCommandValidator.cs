using FluentValidation;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePhoneNumber;

public class ChangePhoneNumberCommandValidator : AbstractValidator<ChangePhoneNumberCommand>
{
    public ChangePhoneNumberCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("The field UserId is required.");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("The field Phone Number is required.")
            .MaximumLength(25).WithMessage("The field Phone Number can't exceed 25 characters.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Must be a valid phone number");
    }
}