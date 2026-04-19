using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.ConfirmBooking;

public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
    }
}