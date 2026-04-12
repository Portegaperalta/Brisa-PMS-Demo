using FluentValidation;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;

public class UpdateCompanyAddressInfoCommandValidator : AbstractValidator<UpdateCompanyAddressInfoCommand>
{
    public UpdateCompanyAddressInfoCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .NotEmpty().WithMessage("The field  CompanyId is required.");
        
        RuleFor(x => x.NewAddress1)
            .NotEmpty().WithMessage("The field Address 1 is required")
            .MaximumLength(200).WithMessage("The field Address 1 can't exceed 200 characters");

        RuleFor(x => x.NewAddress2)
            .MaximumLength(200).WithMessage("The field Address 2 can't exceed 200 characters");
        
        RuleFor(x => x.NewCity)
            .NotEmpty().WithMessage("The field City is required")
            .MaximumLength(100).WithMessage("The field City can't exceed 100 characters");
        
        RuleFor(x => x.NewProvince)
            .NotEmpty().WithMessage("The field Province is required")
            .MaximumLength(100).WithMessage("The field Province can't exceed 100 characters");

        RuleFor(x => x.NewZipCode)
            .NotEmpty().WithMessage("The field ZipCode is required")
            .Matches(@"^\d+$").WithMessage("Zip code must contain only numbers.")
            .MaximumLength(10).WithMessage("The field ZipCode can't exceed 10 characters");
    }
}