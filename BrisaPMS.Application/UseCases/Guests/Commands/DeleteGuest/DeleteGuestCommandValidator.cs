using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.DeleteGuest;

public class DeleteGuestCommandValidator : AbstractValidator<DeleteGuestCommand>
{
    public DeleteGuestCommandValidator()
    {
        RuleFor(command => command.Id).NotEmpty().WithMessage("The field BookingId is required.");
    }
}