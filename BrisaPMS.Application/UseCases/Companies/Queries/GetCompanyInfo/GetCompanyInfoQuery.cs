using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Companies.Queries.GetCompanyInfo;

public class GetCompanyInfoQuery : IRequest<CompanyDto>
{
    public required Guid CompanyId { get; set; }
}