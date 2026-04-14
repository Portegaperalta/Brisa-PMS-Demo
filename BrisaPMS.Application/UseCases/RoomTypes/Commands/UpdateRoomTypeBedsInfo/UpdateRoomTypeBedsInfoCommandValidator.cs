using BrisaPMS.Domain.RoomTypes;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBedsInfo;

public class UpdateRoomTypeBedsInfoCommandValidator : AbstractValidator<UpdateRoomTypeBedsInfoCommand>
{
    public UpdateRoomTypeBedsInfoCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .NotEmpty().WithMessage("The field RoomTypeId is required.");
        
        RuleFor(x => x.BedType)
            .NotEmpty().WithMessage("The field Bed Type is required.")
            .MaximumLength(30).WithMessage("The field Bed Type can't exceed 50 characters.")
            .Must(x => Enum.IsDefined(typeof(BedType), x))
            .WithMessage("Bed type not supported.");
    }
}