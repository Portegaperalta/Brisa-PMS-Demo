using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.RoomTypes.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Queries.GetRoomTypeById;

public class GetRoomTypeByIdUseCase : IRequestHandler<GetRoomTypeByIdQuery, RoomTypeDto>
{
    private readonly IRoomTypesRepository  _roomTypesRepository;
    
    public GetRoomTypeByIdUseCase(IRoomTypesRepository roomTypesRepository)
    {
        _roomTypesRepository = roomTypesRepository;
    }

    public async Task<RoomTypeDto> Handle(GetRoomTypeByIdQuery query)
    {
        var roomType = await _roomTypesRepository.GetById(query.RoomTypeId);

        if (roomType is null)
            throw new NotFoundException("Room Type", query.RoomTypeId);

        return roomType.ToDto();
    }
}