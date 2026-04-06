using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelCheckOutPolicy;

public class UpdateHotelCheckOutPolicyValidator : AbstractValidator<UpdateHotelCheckOutPolicyCommand>
{
    public UpdateHotelCheckOutPolicyValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Field Hotel Id is required");
        
        RuleFor(x => x.CheckInTime)
            .NotEmpty().WithMessage("The field  CheckInTime is required");
        
        RuleFor(x => x.CheckOutTime)
            .NotEmpty().WithMessage("The field  CheckOutTime is required");
    }
}