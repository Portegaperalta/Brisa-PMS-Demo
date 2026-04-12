using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Companies.Queries.GetCompanyInfo;

public class GetCompanyInfoUseCase : IRequestHandler<GetCompanyInfoQuery, CompanyDto>
{
    private readonly ICompaniesRepository _companiesRepository;

    public GetCompanyInfoUseCase(ICompaniesRepository companiesRepository)
    {
        _companiesRepository = companiesRepository;
    }

    public async Task<CompanyDto> Handle(GetCompanyInfoQuery query)
    {
        var company = await _companiesRepository.GetById(query.CompanyId);
        
        if (company is null)
            throw new NotFoundException("Company", query.CompanyId);

        return company.ToDto();
    }
}