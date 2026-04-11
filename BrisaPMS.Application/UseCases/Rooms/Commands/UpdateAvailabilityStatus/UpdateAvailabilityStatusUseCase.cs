using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.UpdateAvailabilityStatus;

public class UpdateAvailabilityStatusUseCase : IRequestHandler<UpdateAvailabilityStatusCommand, bool>
{
    private readonly IRoomsRepository  _roomsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAvailabilityStatusUseCase(IRoomsRepository roomsRepository, IUnitOfWork unitOfWork)
    {
        _roomsRepository = roomsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateAvailabilityStatusCommand command)
    {
        var room = await _roomsRepository.GetById(command.RoomId);
        
        if (room is null)
            throw new NotFoundException("Room", command.RoomId);

        var newAvailabilityStatus = Enum.Parse<RoomAvailabilityStatus>(command.AvailabilityStatus);
        
        room.UpdateAvailabilityStatus(newAvailabilityStatus);

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