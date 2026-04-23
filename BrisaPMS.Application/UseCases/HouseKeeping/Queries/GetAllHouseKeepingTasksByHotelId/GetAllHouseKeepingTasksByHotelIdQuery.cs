using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasksByHotelId;

public class GetAllHouseKeepingTasksByHotelIdQuery : IRequest<List<HouseKeepingTaskDto>>
{
    public required Guid HotelId { get; set; }
}