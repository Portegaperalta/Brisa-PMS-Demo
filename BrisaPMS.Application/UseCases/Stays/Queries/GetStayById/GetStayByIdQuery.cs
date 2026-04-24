using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Queries.GetStayById;

public class GetStayByIdQuery : IRequest<StayDto>
{
    public required Guid StayId { get; set; }
}