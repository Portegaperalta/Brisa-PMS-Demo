using FluentValidation;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangeEmail;

public class ChangeEmailCommandValidator : AbstractValidator<ChangeEmailCommand>
{
    public ChangeEmailCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("The field UserId is required.");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The field Email is required.")
            .MaximumLength(254).WithMessage("The field Email can't exceed 254 characters.")
            .EmailAddress().WithMessage("Must be a valid email address.");
    }
}