using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyBrandInfo;

public class UpdateCompanyBrandInfoCommand : IRequest<bool>
{
    public required Guid CompanyId { get; set; }
    public required string NewLegalName { get; set; }
    public required string NewCommercialName { get; set; }
    public string? NewLogoUrl { get; set; }
}