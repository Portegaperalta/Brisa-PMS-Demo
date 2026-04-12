using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyAddressInfo;

public class UpdateCompanyAddressInfoCommand : IRequest<bool>
{
    public required Guid CompanyId { get; init; }
    public required string NewAddress1 { get; set; }
    public string? NewAddress2 { get; set; }
    public required string NewCity { get; set; }
    public required string NewProvince { get; set; }
    public required string NewZipCode { get; set; }
}