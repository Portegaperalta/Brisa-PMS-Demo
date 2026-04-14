using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.RoomTypes;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeOccupancyPolicy;

public class UpdateRoomTypeOccupancyPolicyUseCase : IRequestHandler<UpdateRoomTypeOccupancyPolicyCommand, bool>
{
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public UpdateRoomTypeOccupancyPolicyUseCase(IRoomTypesRepository roomTypesRepository, IUnitOfWork unitOfWork)
    {
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateRoomTypeOccupancyPolicyCommand command)
    {
        var roomType = await _roomTypesRepository.GetById(command.RoomTypeId);

        if (roomType is null)
            throw new NotFoundException("Room Type", command.RoomTypeId);

        var newOccupancyPolicy = new OccupancyPolicy
        (
            command.MaxOccupancyAdults,
            command.MaxOccupancyChildren
        );
        
        roomType.UpdateOccupancyPolicy(newOccupancyPolicy);

        try
        {
            await _roomTypesRepository.Update(roomType);
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