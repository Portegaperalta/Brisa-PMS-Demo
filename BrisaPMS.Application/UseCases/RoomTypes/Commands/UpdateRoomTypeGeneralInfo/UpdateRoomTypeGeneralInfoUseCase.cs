using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeGeneralInfo;

public class UpdateRoomTypeGeneralInfoUseCase : IRequestHandler<UpdateRoomTypeGeneralInfoCommand, bool>
{
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork  _unitOfWork;

    public UpdateRoomTypeGeneralInfoUseCase(IRoomTypesRepository roomTypesRepository, IUnitOfWork unitOfWork)
    {
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateRoomTypeGeneralInfoCommand command)
    {
        var roomType = await _roomTypesRepository.GetById(command.RoomTypeId);

        if (roomType is null)
            throw new NotFoundException("Room Type", command.RoomTypeId);
        
        roomType.UpdateName(command.Name);
        
        if (command.Description is not null)
            roomType.UpdateDescription(command.Description);

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