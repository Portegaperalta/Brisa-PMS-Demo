using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Rooms.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Queries.GetAllRoomsByHotelId;

public class GetAllRoomsByHotelIdUseCase : IRequestHandler<GetAllRoomsByHotelIdQuery, List<RoomDto>>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly IHotelsRepository _hotelsRepository;

    public GetAllRoomsByHotelIdUseCase(IRoomsRepository roomsRepository, IHotelsRepository hotelsRepository)
    {
        _roomsRepository = roomsRepository;
        _hotelsRepository = hotelsRepository;
    }

    public async Task<List<RoomDto>> Handle(GetAllRoomsByHotelIdQuery query)
    {
        var hotelExists = await _hotelsRepository.Exists(query.HotelId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", query.HotelId);
        
        var rooms = await _roomsRepository.GetAllByHotelId(query.HotelId);
        var roomsDtos = rooms.Select(r => r.ToDto()).ToList();
        return roomsDtos;
    }
}