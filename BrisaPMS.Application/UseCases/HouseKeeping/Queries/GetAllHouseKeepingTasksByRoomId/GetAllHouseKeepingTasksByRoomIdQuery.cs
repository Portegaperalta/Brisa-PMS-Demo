using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasksByRoomId;

public class GetAllHouseKeepingTasksByRoomIdQuery : IRequest<List<HouseKeepingTaskDto>>
{
    public required Guid RoomId { get; set; }
}