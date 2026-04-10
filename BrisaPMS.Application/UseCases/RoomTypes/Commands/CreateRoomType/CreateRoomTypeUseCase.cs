using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeUseCase : IRequestHandler<CreateRoomTypeCommand, Guid>
{
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateRoomTypeUseCase(IRoomTypesRepository roomTypesRepository, IUnitOfWork unitOfWork)
    {
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateRoomTypeCommand command)
    {
        var bedType = Enum.Parse<BedType>(command.BedType);

        var roomType = new RoomType
        (
            command.Name,
            command.BaseRate,
            command.TotalBeds,
            bedType,
            command.MaxOccupancyAdults,
            command.MaxOccupancyChildren,
            command.Description
        );

        try
        {
            var result = await _roomTypesRepository.Create(roomType);
            await _unitOfWork.Persist();
            return result.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}