using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.CompleteBooking;

public class CompleteBookingCommandValidator : AbstractValidator<CompleteBookingCommand>
{
    public CompleteBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
    }
}