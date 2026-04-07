using System.Data;
using BrisaPMS.Domain.Shared.Enums;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.CreateHotel
{
    public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
    {
        public CreateHotelCommandValidator()
        {
            RuleFor(x => x.LegalName)
                .NotEmpty().WithMessage("The field Legal Name is required")
                .MaximumLength(250).WithMessage("The field Legal Name can't exceed 250 characters");

            RuleFor(x => x.CommercialName)
                .NotEmpty().WithMessage("The field Commercial Name is required")
                .MaximumLength(250).WithMessage("The field Commercial Name can't exceed 250 characters");;

            RuleFor(x => x.BusinessEmail)
                 .NotEmpty().WithMessage("The field Business Email is required")
                 .EmailAddress().WithMessage("The email must be a valid email address")
                 .MaximumLength(254).WithMessage("The field Business Email can't exceed 254 characters");

            RuleFor(x => x.BusinessPhoneNumber)
                 .NotEmpty().WithMessage("The field Business Phone Number is required")
                 .Matches(@"^\+?[1-9]\d{{1,14}}$").WithMessage("Must be a valid phone number")
                 .MaximumLength(25).WithMessage("The field Business Phone Number can't exceed 25 characters");

            RuleFor(x => x.Address1)
                 .NotEmpty().WithMessage("The field Address is required")
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

            RuleFor(x => x.CheckInTime)
                 .NotEmpty().WithMessage("The field Check-In Time is required");

            RuleFor(x => x.CheckOutTime)
                .NotEmpty().WithMessage("The field Check-Out Time is required");


            RuleFor(x => x.DefaultCurrencyCode)
                .MaximumLength(3).WithMessage("The field Default CurrencyCode can't exceed 3 characters")
                .Must(x => Enum.IsDefined(typeof(CurrencyCode), x)).WithMessage("Currency code not supported");

            RuleFor(x => x.ItbisRate)
                 .NotEmpty().WithMessage("The field Itbis Rate is required")
                 .GreaterThan(0).WithMessage("The field Itbis Rate can't be negative")
                 .LessThan(100).WithMessage("The field Itbis Rate can't be greater than 100");

            RuleFor(x => x.ServiceChargeRate)
                 .NotEmpty().WithMessage("The field Service Charge Rate is required")
                 .GreaterThan(0).WithMessage("The field Service Charge Rate can't be negative")
                 .LessThan(100).WithMessage("The field Service Charge Rate can't be greater than 100");
        }
    }
}
