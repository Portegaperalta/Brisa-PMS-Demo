using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Shared.Exceptions;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.ChangeAssignedRoom;

public class ChangeAssignedRoomUseCase : IRequestHandler<ChangeAssignedRoomCommand, bool>
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangeAssignedRoomUseCase(IBookingsRepository bookingsRepository, IRoomsRepository roomsRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingsRepository = bookingsRepository;
        _roomsRepository = roomsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(ChangeAssignedRoomCommand command)
    {
        var booking = await _bookingsRepository.GetById(command.BookingId);

        if (booking is null)
            throw new NotFoundException("Booking", command.BookingId);

        var currentRoom = await _roomsRepository.GetById(booking.RoomId);
        var newRoom = await _roomsRepository.GetById(command.RoomId);

        if (newRoom is null)
            throw new NotFoundException("Room", command.RoomId);

        switch (newRoom.AvailabilityStatus)
        {
            case RoomAvailabilityStatus.Reserved:
                throw new BusinessRuleException("Requested room is already reserved");

            case RoomAvailabilityStatus.Occupied:
                throw new BusinessRuleException("Requested room is currently occupied");

            case RoomAvailabilityStatus.OutOfService:
                throw new BusinessRuleException("Requested room is currently out of service");

            case RoomAvailabilityStatus.Available:

            default:
                booking.ChangeAssignedRoom(command.RoomId);
                newRoom.UpdateAvailabilityStatus(RoomAvailabilityStatus.Reserved);
                currentRoom!.UpdateAvailabilityStatus(RoomAvailabilityStatus.Available);
                break;
        }

        try
        {
            await _roomsRepository.Update(currentRoom);
            await _roomsRepository.Update(newRoom);
            await _bookingsRepository.Update(booking);
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