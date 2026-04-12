using FluentValidation;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;

public class UpdateCompanyBrandInfoCommandValidator : AbstractValidator<UpdateCompanyBrandInfoCommand>
{
    public UpdateCompanyBrandInfoCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("The field CompanyId is required.");
        
        RuleFor(x => x.NewLegalName)
            .NotEmpty().WithMessage("The field Legal Name is required.")
            .MaximumLength(250).WithMessage("The field Legal Name can't exceed 250 characters.");
        
        RuleFor(x => x.NewCommercialName)
            .NotEmpty().WithMessage("The field Commercial Name is required.")
            .MaximumLength(250).WithMessage("The field Commercial Name can't exceed 250 characters.");
        
        RuleFor(x => x.NewLogoUrl)
            .MaximumLength(2048).WithMessage("The field Logo Url can't exceed 2048 characters.")
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out var uri)
                         && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
            .WithMessage("A valid HTTP/HTTPS URL is required");
    }
}