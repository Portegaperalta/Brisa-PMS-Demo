using BrisaPMS.Domain.Rooms;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.UpdateAvailabilityStatus;

public class UpdateAvailabilityStatusCommandValidator : AbstractValidator<UpdateAvailabilityStatusCommand>
{
    public UpdateAvailabilityStatusCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("The field RoomId is required.");
        
        RuleFor(x => x.AvailabilityStatus)
            .NotEmpty().WithMessage("The field Availability Status is required.")
            .MaximumLength(11).WithMessage("The field Availability Status can't exceed 11 characters.")
            .Must(x => Enum.IsDefined(typeof(RoomAvailabilityStatus), x))
            .WithMessage("Availability status not supported.");
    }
}