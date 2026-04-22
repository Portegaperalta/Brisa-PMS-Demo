using BrisaPMS.Domain.Users;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangeRole;

public class ChangeRoleCommandValidator : AbstractValidator<ChangeRoleCommand>
{
    public ChangeRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("The field UserId is required.");
        
        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("The field Role is required.")
            .Must(x => Enum.IsDefined(typeof(UserRole), x));
    }
}