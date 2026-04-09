using BrisaPMS.Domain.Rooms;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("The field HotelId is required");

        RuleFor(x => x.RoomTypeId)
            .NotEmpty().WithMessage("The field RoomTypeId is required");
        
        RuleFor(x => x.Number)
            .NotEmpty().WithMessage("The field Number is required")
            .MaximumLength(100).WithMessage("The field Number cannot exceed 100 characters");

        RuleFor(x => x.Floor)
            .NotEmpty().WithMessage("The field Floor is required");
        
        RuleFor(x => x.AvailabilityStatus)
            .NotEmpty().WithMessage("The field Availability Status is required")
            .Must(x => Enum.IsDefined<RoomAvailabilityStatus>(x))
            .WithMessage("Room availability status not supported");
        
        RuleFor(x => x.HygieneStatus)
            .NotEmpty().WithMessage("The field Hygiene Status is required")
            .Must(x => Enum.IsDefined<RoomHygieneStatus>(x))
            .WithMessage("Room hygiene status not supported");
    }
}