using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyRnc;

public class UpdateCompanyRncUseCase
{
    private readonly ICompaniesRepository  _companiesRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public UpdateCompanyRncUseCase(ICompaniesRepository companiesRepository, IUnitOfWork unitOfWork)
    {
        _companiesRepository = companiesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCompanyRncCommand command)
    {
        var company = await _companiesRepository.GetById(command.CompanyId);

        if (company is null)
            throw new NotFoundException("Company", command.CompanyId);

        var newRnc = new Rnc(command.NewRnc);
        company.UpdateRnc(newRnc);

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