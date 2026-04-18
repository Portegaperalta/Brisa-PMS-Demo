using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;

public class ChangeAssignedRoomCommandValidator : AbstractValidator<ChangeAssignedRoomCommand>
{
    public ChangeAssignedRoomCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("The field BookingId is required");
        
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("The field RoomId is required");
    }
}