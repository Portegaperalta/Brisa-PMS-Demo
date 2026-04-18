using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.RevokeGuestVip;

public class RevokeGuestVipCommandValidator : AbstractValidator<RevokeGuestVipCommand>
{
    public RevokeGuestVipCommandValidator()
    {
        RuleFor(command => command.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");
    }
}
