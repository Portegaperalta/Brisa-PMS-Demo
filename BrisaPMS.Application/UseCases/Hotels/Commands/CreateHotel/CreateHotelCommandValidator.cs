using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel
{
    public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
    {
        public CreateHotelCommandValidator()
        {
            RuleFor(p => p.LegalName)
                .NotEmpty().WithMessage("The field Legal Name is required");

            RuleFor(p => p.LegalName)
                .NotEmpty().WithMessage("The field Commercial Name is required");

            RuleFor(p => p.BusinessEmail)
                 .NotEmpty().WithMessage("The field Business Email is required");

            RuleFor(p => p.BusinessPhoneNumber)
                 .NotEmpty().WithMessage("The field Business Phone Number is required");

            RuleFor(p => p.Address)
                 .NotEmpty().WithMessage("The field Address is required");

            RuleFor(p => p.CheckInOutTimes)
                 .NotEmpty().WithMessage("The field CheckInOutTimes is required");

            RuleFor(p => p.ItbisRate)
                 .NotEmpty().WithMessage("The field Itbis Rate is required");

            RuleFor(p => p.ServiceChargeRate)
                 .NotEmpty().WithMessage("The field Itbis Rate is required");
        }
    }
}
