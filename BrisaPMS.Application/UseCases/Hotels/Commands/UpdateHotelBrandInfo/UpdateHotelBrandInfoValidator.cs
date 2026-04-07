using FluentValidation;

namespace BrisaPMS.Application.UseCases.Hotels.Commands.UpdateHotelBrandInfo;

public class UpdateHotelBrandInfoValidator : AbstractValidator<UpdateHotelBrandInfoCommand>
{
    public UpdateHotelBrandInfoValidator()
    {
        RuleFor(x => x.HotelId)
            .NotEmpty().WithMessage("Field Id is required");
        
        RuleFor(x => x.LegalName)
            .NotEmpty().WithMessage("Field LegalName is required")
            .MaximumLength(250).WithMessage("Field Legal Name must not exceed 250 characters");
        
        RuleFor(x => x.CommercialName)
            .NotEmpty().WithMessage("Field Commercial Name is required")
            .MaximumLength(250).WithMessage("Field Commercial Name must not exceed 250 characters");
        
        RuleFor(x => x.LogoUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("A valid HTTP/HTTPS URL is required");
    }
}