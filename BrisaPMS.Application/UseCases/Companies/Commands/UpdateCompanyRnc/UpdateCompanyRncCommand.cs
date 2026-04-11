using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Companies.Commands.UpdateCompanyRnc;

public class UpdateCompanyRncCommand : IRequest<bool>
{
    public required Guid CompanyId { get; set; }
    public required string NewRnc {get; set;}
}