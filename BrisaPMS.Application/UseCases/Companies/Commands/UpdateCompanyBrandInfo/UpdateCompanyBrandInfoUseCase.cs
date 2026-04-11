using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;

public class UpdateCompanyBrandInfoUseCase : IRequestHandler<UpdateCompanyBrandInfoCommand, bool>
{
    private readonly ICompaniesRepository _companyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCompanyBrandInfoUseCase(ICompaniesRepository companyRepository, IUnitOfWork unitOfWork)
    {
        _companyRepository = companyRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCompanyBrandInfoCommand command)
    {
        var company = await _companyRepository.GetById(command.CompanyId);
        
        if (company is null)
            throw new NotFoundException("Company", command.CompanyId);
        
        company.ChangeLegalName(command.NewLegalName);
        company.ChangeCommercialName(command.NewCommercialName);

        if (command.NewLogoUrl is not null)
        {
            var newLogoUrl = new Url(command.NewLogoUrl);
            company.UpdateLogoUrl(newLogoUrl);
        }

        try
        {
            await _companyRepository.Update(company);
            await _unitOfWork.Persist();
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}