using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRooms;

public class GetAllRoomsUseCase : IRequestHandler<GetAllRoomsQuery, List<RoomDto>>
{
    private readonly IRoomsRepository _roomsRepository;
    
    public GetAllRoomsUseCase(IRoomsRepository roomsRepository) { _roomsRepository = roomsRepository; }

    public async Task<List<RoomDto>> Handle(GetAllRoomsQuery query)
    {
        var rooms = await _roomsRepository.GetAll();
        var roomsDtos = rooms.Select(r => r.ToDto()).ToList();
        return roomsDtos;
    }
}