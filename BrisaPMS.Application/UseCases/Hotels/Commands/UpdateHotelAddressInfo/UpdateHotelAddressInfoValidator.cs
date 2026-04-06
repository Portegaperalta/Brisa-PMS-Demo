using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelAddressInfo;

public class UpdateHotelAddressInfoValidator : AbstractValidator<UpdateHotelAddressInfoCommand>
{
    public UpdateHotelAddressInfoValidator()
    {
        RuleFor(x => x.Address1)
            .NotEmpty().WithMessage("The field Address 1 is required")
            .MaximumLength(200).WithMessage("The field Address 1 can't exceed 200 characters");

        RuleFor(x => x.Address2)
            .MaximumLength(200).WithMessage("The field Address 2 can't exceed 200 characters");
        
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("The field City is required")
            .MaximumLength(100).WithMessage("The field City can't exceed 100 characters");
        
        RuleFor(x => x.Province)
            .NotEmpty().WithMessage("The field Province is required")
            .MaximumLength(100).WithMessage("The field Province can't exceed 100 characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("The field ZipCode is required")
            .Matches(@"^\d+$").WithMessage("Zip code must contain only numbers.")
            .MaximumLength(10).WithMessage("The field ZipCode can't exceed 10 characters");
    }
}