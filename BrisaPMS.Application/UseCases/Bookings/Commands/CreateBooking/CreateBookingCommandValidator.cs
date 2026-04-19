using BrisaPMS.Domain.Bookings;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.CreateBooking;

public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("The field HotelId is required.");
        
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("The field RoomId is required.");
        
        RuleFor(x => x.GuestId)
            .NotEmpty().WithMessage("The field GuestId is required.");
        
        RuleFor(x => x.Source)
            .NotEmpty().WithMessage("The field Booking Source is required.")
            .MaximumLength(200).WithMessage("The field Booking Source can't exceed 200 characters.")
            .Must(x => Enum.IsDefined(typeof(BookingSource), x)).WithMessage("Booking Source not supported");
        
        RuleFor(x => x.NumberOfAdults)
            .NotEmpty().WithMessage("The field Number Of Adults is required.")
            .GreaterThanOrEqualTo(1).WithMessage("Booking must include at least 1 Adult.")
            .LessThanOrEqualTo(10).WithMessage("Booking can't exceed 10 Adults.");
        
        RuleFor(x => x.NumberOfChildren)
            .NotEmpty().WithMessage("The field Number Of Children is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Number of children can't be negative.")
            .LessThanOrEqualTo(10).WithMessage("Booking can't exceed 10 Children.");

        RuleFor(x => x.CheckInTime)
            .NotEmpty().WithMessage("The field Check-In Time is required.")
            .LessThan(x => x.CheckOutTime)
            .WithMessage("Check-In Time can't be later than Check-Out Time.");
        
        RuleFor(x => x.CheckOutTime)
            .NotEmpty().WithMessage("The field Check-Out Time is required.")
            .GreaterThan(x => x.CheckInTime)
            .WithMessage("Check-Out Time can't be earlier than Check-In Time.");
        
        RuleFor(x => x.SpecialRequests)
            .MaximumLength(500).WithMessage("The field Special Requests can't exceed 500 characters.");
        
        RuleFor(x => x.TotalPrice)
            .NotEmpty().WithMessage("The field Total Price is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Total Price can't be negative.");
    }
}