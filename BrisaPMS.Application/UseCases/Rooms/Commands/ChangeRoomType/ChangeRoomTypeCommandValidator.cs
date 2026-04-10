using FluentValidation;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.ChangeRoomType;

public class ChangeRoomTypeCommandValidator : AbstractValidator<ChangeRoomTypeCommand>
{
    public ChangeRoomTypeCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("The field RoomId is required.");
        
        RuleFor(x => x.RoomTypeId)
            .NotEmpty().WithMessage("The field RoomTypeId is required.");
    }
}