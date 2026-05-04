using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.DeleteBooking;

public class DeleteBookingCommandValidator : AbstractValidator<DeleteBookingCommand>
{
    public DeleteBookingCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithMessage("The field BookingId is required.");
    }
}