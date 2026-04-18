using BrisaPMS.Domain.Bookings;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.ChangeBookingSource;

public class ChangeBookingSourceCommandValidator : AbstractValidator<ChangeBookingSourceCommand>
{
    public ChangeBookingSourceCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
        
        RuleFor(x => x.Source)
            .NotEmpty().WithMessage("The field Booking Source is required.")
            .MaximumLength(200).WithMessage("The field Booking Source can't exceed 200 characters.")
            .Must(x => Enum.IsDefined(typeof(BookingSource), x)).WithMessage("Booking Source not supported");
    }
}