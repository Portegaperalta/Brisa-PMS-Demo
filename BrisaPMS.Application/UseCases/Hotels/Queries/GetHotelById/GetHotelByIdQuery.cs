using BrisaPMS.Application.UseCases.Hotels.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;

public class GetHotelByIdQuery : IRequest<HotelDto>
{
    public required Guid HotelId { get; set; }
}