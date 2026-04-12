using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyContactInfo;

public class UpdateCompanyContactInfoUseCase : IRequestHandler<UpdateCompanyContactInfoCommand, bool>
{
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public UpdateCompanyContactInfoUseCase(ICompaniesRepository companiesRepository, IUnitOfWork unitOfWork)
    {
        _companiesRepository = companiesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCompanyContactInfoCommand command)
    {
        var company = await _companiesRepository.GetById(command.CompanyId);

        if (company is null)
            throw new NotFoundException("Company", command.CompanyId);

        var newBusinessEmail = new Email(command.NewBusinessEmail);
        var newBusinessPhoneNumber = new PhoneNumber(command.NewBusinessPhoneNumber);
        
        company.UpdateBusinessEmail(newBusinessEmail);
        company.UpdateBusinessPhone(newBusinessPhoneNumber);

        try
        {
            await _companiesRepository.Update(company);
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