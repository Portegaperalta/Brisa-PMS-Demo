using BrisaPMS.Domain.Users;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Users.Commands.ChangePreferredLanguage;

public class ChangePreferredLanguageCommandValidator : AbstractValidator<ChangePreferredLanguageCommand>
{
    public ChangePreferredLanguageCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("The field UserId is required.");
        
        RuleFor(x => x.PreferredLanguage)
            .NotEmpty().WithMessage("The field Preferred Language is required.")
            .Must(x => Enum.IsDefined(typeof(UserPreferredLanguage), x!));
    }
}