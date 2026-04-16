using BrisaPMS.Domain.Guest;
using BrisaPMS.Domain.Shared.Enums;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Guests.Commands.CreateGuest;

public class CreateGuestCommandValidator : AbstractValidator<CreateGuestCommand>
{
    public CreateGuestCommandValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("The field Hotel Id is required.");
        
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("The field First Name is required.")
            .MaximumLength(250).WithMessage("The field First Name can't exceed 250 characters.");
        
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("The field Last Name is required.")
            .MaximumLength(250).WithMessage("The field Last Name can't exceed 250 characters.");
        
        RuleFor(x => x.DocumentType)
            .NotEmpty().WithMessage("The field DocumentType is required.")
            .MaximumLength(50).WithMessage("The field Document Type can't exceed 30 characters.")
            .Must(x => Enum.IsDefined(typeof(GuestDocumentType), x))
            .WithMessage("Document type not supported.");
        
        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("The field Document Number is required.")
            .MaximumLength(250).WithMessage("The field Document Number can't exceed 250 characters.");
        
        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("The field Country can't exceed 100 characters.");
        
        RuleFor(x => x.Rnc)
            .MinimumLength(9).WithMessage("The field  Rnc must be minimum 9 characters long.")
            .MaximumLength(11).WithMessage("The field  Rnc can't exceed 11 characters.")
            .Matches(@"^[0-9]+(-[0-9]+)*$")
            .WithMessage("RNC must contain only numbers and dashes");
        
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("The field Email is required.")
            .EmailAddress().WithMessage("Must be a valid email address.")
            .MaximumLength(254).WithMessage("The field Email can't exceed 254 characters.");
        
        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("The field Phone Number is required.")
            .MaximumLength(25).WithMessage("The field Phone Number can't exceed 25 characters.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Must be a valid phone number");
        
        RuleFor(x => x.PreferredCurrency)
            .NotEmpty().WithMessage("The field Preferred Currency is required.")
            .MaximumLength(3).WithMessage("The field Preferred Currency can't exceed 3 characters.")
            .Must(x => Enum.IsDefined(typeof(CurrencyCode), x))
            .WithMessage("Currency not supported.");

        RuleFor(x => x.IsVip)
            .NotEmpty().WithMessage("The field Is Vip is required.");
        
        RuleFor(x => x.Notes)
            .MaximumLength(500).WithMessage("The field Notes can't exceed 500 characters.");
    }
}