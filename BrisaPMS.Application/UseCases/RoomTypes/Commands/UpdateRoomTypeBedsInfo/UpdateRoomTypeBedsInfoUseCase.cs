using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.RoomTypes;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.UpdateRoomTypeBedsInfo;

public class UpdateRoomTypeBedsInfoUseCase : IRequestHandler<UpdateRoomTypeBedsInfoCommand, bool>
{
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateRoomTypeBedsInfoUseCase(IRoomTypesRepository roomTypesRepository, IUnitOfWork unitOfWork)
    {
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateRoomTypeBedsInfoCommand command)
    {
        var roomType = await _roomTypesRepository.GetById(command.RoomTypeId);

        if (roomType is null)
            throw new NotFoundException("Room Type", command.RoomTypeId);
        
        var bedType = Enum.Parse<BedType>(command.BedType);
        var newBeds = new RoomBed(bedType, command.NumberOfBeds);
        
        roomType.UpdateRoomBeds(newBeds);

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