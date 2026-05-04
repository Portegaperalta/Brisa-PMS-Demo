using FluentValidation;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.DeleteRoom;

public class DeleteRoomCommandValidator : AbstractValidator<DeleteRoomCommand>
{
    public DeleteRoomCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithMessage("The field RoomId is required.");
    }
}