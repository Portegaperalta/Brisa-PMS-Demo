using FluentValidation;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.DeleteRoomType;

public class DeleteRoomTypeCommandValidator : AbstractValidator<DeleteRoomTypeCommand>
{
    public DeleteRoomTypeCommandValidator()
    {
        RuleFor(r => r.Id).NotEmpty().WithMessage("The field RoomTypeId is required.");
    }
}