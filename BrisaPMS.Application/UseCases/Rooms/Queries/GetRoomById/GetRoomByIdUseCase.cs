using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Queries.GetRoomById;

public class GetRoomByIdUseCase : IRequestHandler<GetRoomByIdQuery, RoomDto>
{
    private readonly IRoomsRepository  _roomsRepository;
    
    public GetRoomByIdUseCase(IRoomsRepository roomsRepository) { _roomsRepository = roomsRepository; }

    public async Task<RoomDto> Handle(GetRoomByIdQuery query)
    {
        var room = await _roomsRepository.GetById(query.RoomId);
        
        if (room is null)
            throw new NotFoundException("Room", query.RoomId);

        return room.ToDto();
    }
}