using FluentValidation;

namespace BrisaPMS.Application.UseCases.Users.Commands.Login
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The field Email is required.")
            .MaximumLength(254).WithMessage("The field Email can't exceed 254 characters.")
            .EmailAddress().WithMessage("Must be a valid email address.");

            RuleFor(x => x.Password)
           .NotEmpty().WithMessage("The field Password is required.");
        }
    }
}