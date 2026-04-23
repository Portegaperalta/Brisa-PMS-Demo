using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetHouseKeepingTaskById;

public class GetHouseKeepingTaskByIdQuery : IRequest<HouseKeepingTaskDto>
{
    public required Guid HouseKeepingTaskId { get; set; }
}