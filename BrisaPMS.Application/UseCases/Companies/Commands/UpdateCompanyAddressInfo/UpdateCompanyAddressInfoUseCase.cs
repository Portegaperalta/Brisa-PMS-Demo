using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;

public class UpdateCompanyAddressInfoUseCase : IRequestHandler<UpdateCompanyAddressInfoCommand, bool>
{
    private readonly ICompaniesRepository _companiesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCompanyAddressInfoUseCase(ICompaniesRepository companiesRepository, IUnitOfWork unitOfWork)
    {
        _companiesRepository = companiesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCompanyAddressInfoCommand command)
    {
        var company = await _companiesRepository.GetById(command.CompanyId);

        if (company is null)
            throw new NotFoundException("Company", command.CompanyId);

        var newAddress = new Address
        (
            command.NewAddress1,
            command.NewAddress2,
            command.NewCity,
            command.NewProvince,
            command.NewZipCode
        );
        
        company.UpdateAddress(newAddress);

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