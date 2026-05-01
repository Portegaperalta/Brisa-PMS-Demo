using FluentValidation;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePassword;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("The field UserId is required.");

        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("The field Password is required.");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("The field Password is required.")
            .MaximumLength(512).WithMessage("The field Password can't exceed 512 characters.")
            .MinimumLength(8).WithMessage("Password must contain at least 8 characters.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character.");
    }
}