using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCheckInOutTimes;

public class UpdateCheckInOutTimesCommandValidator : AbstractValidator<UpdateCheckInOutTimesCommand>
{
    public UpdateCheckInOutTimesCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
        
        RuleFor(x => x.CheckInTime)
            .NotEmpty().WithMessage("The field Check-In Time is required.")
            .LessThan(x => x.CheckOutTime)
            .WithMessage("Check-In Time can't be later than Check-Out Time.");
        
        RuleFor(x => x.CheckOutTime)
            .NotEmpty().WithMessage("The field Check-Out Time is required.")
            .GreaterThan(x => x.CheckInTime)
            .WithMessage("Check-Out Time can't be earlier than Check-In Time.");
    }
}