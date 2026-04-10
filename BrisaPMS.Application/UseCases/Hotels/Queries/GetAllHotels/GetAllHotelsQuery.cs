using BrisaPMS.Application.UseCases.Hotels.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Queries.GetAllHotels;

public class GetAllHotelsQuery : IRequest<List<HotelDto>>
{
}