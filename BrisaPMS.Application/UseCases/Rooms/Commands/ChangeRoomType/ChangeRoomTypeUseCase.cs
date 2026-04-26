using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.ChangeRoomType;

public class ChangeRoomTypeUseCase : IRequestHandler<ChangeRoomTypeCommand, bool>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeRoomTypeUseCase(IRoomsRepository roomsRepository, IRoomTypesRepository roomTypesRepository,
        IUnitOfWork unitOfWork)
    {
        _roomsRepository = roomsRepository;
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangeRoomTypeCommand command)
    {
        var room = await _roomsRepository.GetById(command.RoomId);
        
        if (room is null)
            throw new NotFoundException("Room", command.RoomId);

        var roomTypeExists = await _roomTypesRepository.Exists(command.RoomTypeId);

        if (roomTypeExists is not true)
            throw new NotFoundException("RoomType", command.RoomTypeId);
        
        room.ChangeRoomType(command.RoomTypeId);

        try
        {
            await _roomsRepository.Update(room);
            await _unitOfWork.Persist();
            return true;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}