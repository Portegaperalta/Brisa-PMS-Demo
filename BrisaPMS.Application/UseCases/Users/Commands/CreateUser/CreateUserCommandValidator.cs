using BrisaPMS.Domain.Users;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Users.Commands.CreateUser;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("The field Role is required.")
            .Must(x => Enum.IsDefined(typeof(UserRole), x));

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("The field First Name is required.")
            .MaximumLength(250).WithMessage("The field First Name can't exceed 250 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("The field Last Name is required.")
            .MaximumLength(250).WithMessage("The field Last Name can't exceed 250 characters.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The field Email is required.")
            .MaximumLength(254).WithMessage("The field Email can't exceed 254 characters.")
            .EmailAddress().WithMessage("Must be a valid email address.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("The field Password is required.")
            .MaximumLength(512).WithMessage("The field Password can't exceed 512 characters.")
            .MinimumLength(8).WithMessage("Password must contain at least 8 characters.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
        
        RuleFor(x => x.PhoneNumber)
            .MaximumLength(25).WithMessage("The field Phone Number can't exceed 25 characters.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Must be a valid phone number");

        RuleFor(x => x.PreferredLanguage)
            .Must(x => Enum.IsDefined(typeof(UserPreferredLanguage), x!));
    }
}