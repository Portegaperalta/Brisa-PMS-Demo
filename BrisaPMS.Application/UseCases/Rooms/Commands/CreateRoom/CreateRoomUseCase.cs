using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Rooms.Commands.CreateRoom;

public class CreateRoomUseCase
{
    private readonly IRoomsRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomUseCase(IRoomsRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateRoomCommand command)
    {
        try
        {
            var room = new Room
            (
                command.HotelId,
                command.RoomTypeId,
                command.Number,
                command.Floor,
                command.AvailabilityStatus,
                command.HygieneStatus
            );

            var response = await _repository.Create(room);
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