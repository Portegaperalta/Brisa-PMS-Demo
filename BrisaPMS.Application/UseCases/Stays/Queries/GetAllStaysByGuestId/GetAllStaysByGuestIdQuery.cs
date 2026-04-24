using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByGuestId;

public class GetAllStaysByGuestIdQuery : IRequest<List<StayDto>>
{
    public required Guid GuestId { get; set; }
}