using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.UpdateHygieneStatus;

public class UpdateHygieneStatusUseCase : IRequestHandler<UpdateHygieneStatusCommand, bool>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public UpdateHygieneStatusUseCase(IRoomsRepository roomsRepository, IUnitOfWork unitOfWork)
    {
        _roomsRepository = roomsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateHygieneStatusCommand command)
    {
        var room = await _roomsRepository.GetById(command.RoomId);
        var newHygieneStatus = Enum.Parse<RoomHygieneStatus>(command.HygieneStatus);
        
        if (room is null)
            throw new NotFoundException("Room", command.RoomId);
        
        room.UpdateHygieneStatus(newHygieneStatus);
            
        if (newHygieneStatus is RoomHygieneStatus.Clean)
        {
            room.UpdateLastCleanedAt();
            room.UpdateLastCleanedBy(command.UserId);
        }

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