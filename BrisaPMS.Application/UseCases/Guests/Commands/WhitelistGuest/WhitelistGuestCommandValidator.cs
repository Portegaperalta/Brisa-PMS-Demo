using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.WhitelistGuest;

public class WhitelistGuestCommandValidator : AbstractValidator<WhitelistGuestCommand>
{
    public WhitelistGuestCommandValidator()
    {
        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");
    }
}
