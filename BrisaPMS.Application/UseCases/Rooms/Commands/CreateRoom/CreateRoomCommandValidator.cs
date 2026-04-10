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
            .NotEmpty().WithMessage("The field Floor is required")
            .GreaterThanOrEqualTo(-200).WithMessage("The field floor can't be less than -200")
            .LessThanOrEqualTo(200).WithMessage("The field floor can't be greater than 200");
        
        RuleFor(x => x.AvailabilityStatus)
            .NotEmpty().WithMessage("The field Availability Status is required")
            .MaximumLength(11).WithMessage("The field Availability Status cannot exceed 11 characters")
            .Must(x => Enum.IsDefined(typeof(RoomAvailabilityStatus), x)) 
            .WithMessage("Availability status not supported");
        
        RuleFor(x => x.HygieneStatus)
            .NotEmpty().WithMessage("The field Hygiene Status is required")
            .MaximumLength(11).WithMessage("The field Hygiene Status cannot exceed 11 characters")
            .Must(x => Enum.IsDefined(typeof(RoomHygieneStatus), x))
            .WithMessage("Hygiene status not supported");
    }
}