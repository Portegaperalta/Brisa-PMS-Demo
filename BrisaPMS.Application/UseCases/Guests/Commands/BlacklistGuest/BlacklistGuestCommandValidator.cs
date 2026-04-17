using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.BlacklistGuest;

public class BlacklistGuestCommandValidator : AbstractValidator<BlacklistGuestCommand>
{
    public BlacklistGuestCommandValidator()
    {
        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");
        
        RuleFor(x => x.BlacklistedReason)
            .NotEmpty().WithMessage("The field Blacklisted Reason is required.")
            .MaximumLength(500).WithMessage("The field Blacklisted Reason can't exceed 500 characters.");
    }
}