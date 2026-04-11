using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyContactInfo;

public class UpdateCompanyContactInfoCommand : IRequest<bool>
{
    public required Guid CompanyId { get; set; }
    public required string NewBusinessEmail { get; set; }
    public required string NewBusinessPhoneNumber { get; set; }
}