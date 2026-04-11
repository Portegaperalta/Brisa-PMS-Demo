using FluentValidation;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.SetAsRestocked;

public class SetAsRestockedCommandValidator : AbstractValidator<SetAsRestockedCommand>
{
    public SetAsRestockedCommandValidator()
    {
        RuleFor(x => x.RoomId)
            .NotEmpty().WithMessage("Field RoomId is required");
    }
}