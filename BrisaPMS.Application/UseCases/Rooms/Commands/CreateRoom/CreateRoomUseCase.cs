using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;

public class CreateRoomUseCase : IRequestHandler<CreateRoomCommand, Guid>
{
    private readonly IRoomsRepository _roomsRepository;
    private readonly IHotelsRepository _hotelsRepository;
    private readonly IRoomTypesRepository _roomTypesRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomUseCase(IRoomsRepository roomsRepository, IHotelsRepository hotelsRepository,
        IRoomTypesRepository roomTypesRepository, IUnitOfWork unitOfWork)
    {
        _roomsRepository = roomsRepository;
        _hotelsRepository = hotelsRepository;
        _roomTypesRepository = roomTypesRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateRoomCommand command)
    {
        var hotelExists = await _hotelsRepository.Exists(command.HotelId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", command.HotelId);

        var roomTypeExists = await _roomTypesRepository.Exists(command.RoomTypeId);

        if (roomTypeExists is not true)
            throw new NotFoundException("Room Type", command.RoomTypeId);

        var availabilityStatus = Enum.Parse<RoomAvailabilityStatus>(command.AvailabilityStatus);
        var hygieneStatus = Enum.Parse<RoomHygieneStatus>(command.HygieneStatus);

        var room = new Room
        (
            command.HotelId,
            command.RoomTypeId,
            command.Number,
            command.Floor,
            availabilityStatus,
            hygieneStatus
        );

        try
        {
            var response = await _roomsRepository.Create(room);
            await _unitOfWork.Persist();
            return response.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}