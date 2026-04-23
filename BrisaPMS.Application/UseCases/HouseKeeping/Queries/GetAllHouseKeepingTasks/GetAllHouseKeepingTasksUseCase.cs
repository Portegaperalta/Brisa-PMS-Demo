using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasks;

public class GetAllHouseKeepingTasksUseCase : IRequestHandler<GetAllHouseKeepingTasksQuery, List<HouseKeepingTaskDto>>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;

    public GetAllHouseKeepingTasksUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
    }

    public async Task<List<HouseKeepingTaskDto>> Handle(GetAllHouseKeepingTasksQuery query)
    {
        var houseKeepingTasks = await _houseKeepingTasksRepository.GetAll();
        var houseKeepingTasksDto =  houseKeepingTasks.Select(t => t.ToDto()).ToList();
        return houseKeepingTasksDto;
    }
}