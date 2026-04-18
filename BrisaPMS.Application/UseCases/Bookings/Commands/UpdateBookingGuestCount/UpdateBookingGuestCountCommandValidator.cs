using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateBookingGuestCount;

public class UpdateBookingGuestCountCommandValidator : AbstractValidator<UpdateBookingGuestCountCommand>
{
    public UpdateBookingGuestCountCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
        
        RuleFor(x => x.NumberOfAdults)
            .NotEmpty().WithMessage("The field Number Of Adults is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Booking must include at least 1 Adult.")
            .LessThanOrEqualTo(10).WithMessage("Booking can't exceed 10 Adults.");
        
        RuleFor(x => x.NumberOfChildren)
            .NotEmpty().WithMessage("The field Number Of Children is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Number of children can't be negative.")
            .LessThanOrEqualTo(10).WithMessage("Booking can't exceed 10 Children.");
    }
}