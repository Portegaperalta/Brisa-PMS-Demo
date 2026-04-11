using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.UpdateRoomNumber;

public class UpdateRoomNumberUseCase : IRequestHandler<UpdateRoomNumberCommand, bool>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomNumberUseCase(IRoomsRepository roomsRepository, IUnitOfWork unitOfWork)
    {
        _roomsRepository = roomsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateRoomNumberCommand command)
    {
        var room = await _roomsRepository.GetById(command.RoomId);
        
        if (room is null)
            throw new NotFoundException("Room", command.RoomId);
        
        room.UpdateNumber(command.Number);

        try
        {
            await _roomsRepository.Update(room);
            await _unitOfWork.Persist();
            return true;
        }
        catch (Exception)
        {
            Console.WriteLine();
            throw;
        }
    }
}