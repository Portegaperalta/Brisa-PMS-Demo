using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasksByRoomId;

public class GetAllHouseKeepingTasksByRoomIdUseCase : 
    IRequestHandler<GetAllHouseKeepingTasksByRoomIdQuery, List<HouseKeepingTaskDto>>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IRoomsRepository _roomsRepository;

    public GetAllHouseKeepingTasksByRoomIdUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository,
        IRoomsRepository roomsRepository)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _roomsRepository = roomsRepository;
    }

    public async Task<List<HouseKeepingTaskDto>> Handle(GetAllHouseKeepingTasksByRoomIdQuery query)
    {
        var roomExists = await _roomsRepository.Exists(query.RoomId);

        if (roomExists is not true)
            throw new NotFoundException("Room", query.RoomId);
        
        var houseKeepingTasks = await _houseKeepingTasksRepository.GetAllByRoomIdAsync(query.RoomId);
        var houseKeepingTasksDtos = houseKeepingTasks.Select(t => t.ToDto()).ToList();
        
        return houseKeepingTasksDtos;
    }
}