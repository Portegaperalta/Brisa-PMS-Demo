using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetHouseKeepingTaskById;

public class GetHouseKeepingTaskByIdUseCase : IRequestHandler<GetHouseKeepingTaskByIdQuery, HouseKeepingTaskDto>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;

    public GetHouseKeepingTaskByIdUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
    }

    public async Task<HouseKeepingTaskDto> Handle(GetHouseKeepingTaskByIdQuery query)
    {
        var houseKeepingTask = await _houseKeepingTasksRepository.GetById(query.HouseKeepingTaskId);

        if (houseKeepingTask is null)
            throw new NotFoundException("HouseKeeping Task", query.HouseKeepingTaskId);
        
        return houseKeepingTask.ToDto();
    }
}