using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestRnc;

public class UpdateGuestRncCommandValidator : AbstractValidator<UpdateGuestRncCommand>
{
    public UpdateGuestRncCommandValidator()
    {
        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");

        RuleFor(x => x.Rnc)
            .NotEmpty().WithMessage("The field Rnc is required.")
            .MinimumLength(9).WithMessage("The field Rnc must be minimum 9 characters long.")
            .MaximumLength(11).WithMessage("The field Rnc can't exceed 11 characters.")
            .Matches(@"^[0-9]+(-[0-9]+)*$")
            .WithMessage("RNC must contain only numbers and dashes");
    }
}