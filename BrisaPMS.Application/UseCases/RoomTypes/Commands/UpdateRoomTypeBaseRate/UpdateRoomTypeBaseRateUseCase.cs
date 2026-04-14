using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Billing;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBaseRate;

public class UpdateRoomTypeBaseRateUseCase : IRequestHandler<UpdateRoomTypeBaseRateCommand, bool>
{
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomTypeBaseRateUseCase(IRoomTypesRepository roomTypesRepository, IUnitOfWork unitOfWork)
    {
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateRoomTypeBaseRateCommand command)
    {
        var roomType = await _roomTypesRepository.GetById(command.RoomTypeId);

        if (roomType is null)
            throw new NotFoundException("Room Type", command.RoomTypeId);

        var newBaseRate = new RoomBaseRate(command.NewBaseRate);
        
        roomType.UpdateBaseRate(newBaseRate);

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