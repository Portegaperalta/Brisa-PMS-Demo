using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByHotelId;

public class GetAllStaysByHotelIdQuery : IRequest<List<StayDto>>
{
    public required Guid HotelId { get; set; }
}