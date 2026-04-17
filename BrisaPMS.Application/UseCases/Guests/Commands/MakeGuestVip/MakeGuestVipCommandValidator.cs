using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.MakeGuestVip;

public class MakeGuestVipCommandValidator : AbstractValidator<MakeGuestVipCommand>
{
    public MakeGuestVipCommandValidator()
    {
        RuleFor(command => command.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");
    }
}