using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateGuestCount;

public class UpdateGuestCountCommandValidator : AbstractValidator<UpdateGuestCountCommand>
{
    public UpdateGuestCountCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");

        RuleFor(x => x.NumberOfAdults)
            .NotEmpty().WithMessage("The field Number Of Adults is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Booking must include at least 1 Adult.")
            .LessThanOrEqualTo(10).WithMessage("Booking can't exceed 10 Adults.");

        RuleFor(x => x.NumberOfChildren)
            .GreaterThanOrEqualTo(0).WithMessage("Number of children can't be negative.")
            .LessThanOrEqualTo(10).WithMessage("Booking can't exceed 10 Children.");
    }
}