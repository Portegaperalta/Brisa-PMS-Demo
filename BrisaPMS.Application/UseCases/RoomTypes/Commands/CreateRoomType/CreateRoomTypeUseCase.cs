using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.RoomTypes.Shared;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.RoomTypes;

namespace BrisaPMS.Application.UseCases.RoomTypes.Commands.CreateRoomType;

public class CreateRoomTypeUseCase : IRequestHandler<CreateRoomTypeCommand, RoomTypeDto>
{
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateRoomTypeUseCase(IRoomTypesRepository roomTypesRepository, IUnitOfWork unitOfWork)
    {
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<RoomTypeDto> Handle(CreateRoomTypeCommand command)
    {
        var bedType = Enum.Parse<BedType>(command.BedType);
        var beds = new RoomBed(bedType, command.TotalBeds);
        var occupancyPolicy = new OccupancyPolicy(command.MaxOccupancyAdults, command.MaxOccupancyChildren);
        var baseRate = new RoomBaseRate(command.BaseRate);

        var roomType = new RoomType
        (
            command.Name,
            baseRate,
            beds,
            occupancyPolicy,
            command.Description
        );

        try
        {
            var result = await _roomTypesRepository.Create(roomType);
            await _unitOfWork.Persist();
            return result.ToDto();
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}