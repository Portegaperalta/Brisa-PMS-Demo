using FluentValidation;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;

public class UpdateRoomTypeGeneralInfoCommandValidator : AbstractValidator<UpdateRoomTypeGeneralInfoCommand>
{
    public UpdateRoomTypeGeneralInfoCommandValidator()
    {
        RuleFor(x => x.RoomTypeId)
            .NotEmpty().WithMessage("The field RoomTypeId is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("The field Name is required.")
            .MaximumLength(100).WithMessage("The field Name can't exceed 100 characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("The field Description can't exceed 500 characters.");
    }
}