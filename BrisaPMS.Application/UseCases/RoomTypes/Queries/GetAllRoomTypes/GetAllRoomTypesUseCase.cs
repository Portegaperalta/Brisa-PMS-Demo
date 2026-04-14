using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.RoomTypes.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Queries.GetAllRoomTypes;

public class GetAllRoomTypesUseCase : IRequestHandler<GetAllRoomTypesQuery, List<RoomTypeDto>>
{
    private readonly IRoomTypesRepository _roomTypesRepository;

    public GetAllRoomTypesUseCase(IRoomTypesRepository roomTypesRepository)
    {
        _roomTypesRepository = roomTypesRepository;
    }

    public async Task<List<RoomTypeDto>> Handle(GetAllRoomTypesQuery query)
    {
        var roomTypes = await _roomTypesRepository.GetAll();
        
        var roomTypesDtos = roomTypes.Select(r => r.ToDto()).ToList();
        return roomTypesDtos;
    }
}