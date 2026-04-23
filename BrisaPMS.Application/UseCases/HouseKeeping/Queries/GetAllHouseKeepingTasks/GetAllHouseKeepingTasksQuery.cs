using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasks;

public class GetAllHouseKeepingTasksQuery : IRequest<List<HouseKeepingTaskDto>>
{
}