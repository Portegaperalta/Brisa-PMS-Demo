using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCancellationReason;

public class UpdateCancellationReasonCommandValidator : AbstractValidator<UpdateCancellationReasonCommand>
{
    public UpdateCancellationReasonCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required.");
        
        RuleFor(x => x.CancellationReason)
            .NotEmpty().WithMessage("The field Cancellation Reason is required.")
            .MaximumLength(255).WithMessage("The field Cancellation Reason can't exceed 255 characters.");
    }
}