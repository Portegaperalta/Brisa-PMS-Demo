using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.HouseKeeping.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Queries.GetAllHouseKeepingTasksByHotelId;

public class GetAllHouseKeepingTasksByHotelIdUseCase : 
    IRequestHandler<GetAllHouseKeepingTasksByHotelIdQuery, List<HouseKeepingTaskDto>>
{
    private readonly IHouseKeepingTasksRepository _houseKeepingTasksRepository;
    private readonly IHotelsRepository _hotelsRepository;

    public GetAllHouseKeepingTasksByHotelIdUseCase(IHouseKeepingTasksRepository houseKeepingTasksRepository,
        IHotelsRepository hotelsRepository)
    {
        _houseKeepingTasksRepository = houseKeepingTasksRepository;
        _hotelsRepository = hotelsRepository;
    }

    public async Task<List<HouseKeepingTaskDto>> Handle(GetAllHouseKeepingTasksByHotelIdQuery query)
    {
        var hotelExists = await _hotelsRepository.Exists(query.HotelId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", query.HotelId);

        var houseKeepingTasks = await _houseKeepingTasksRepository.GetByHotelIdAsync(query.HotelId);
        var houseKeepingTasksDtos = houseKeepingTasks.Select(t => t.ToDto()).ToList();
        
        return houseKeepingTasksDtos;
    }
}