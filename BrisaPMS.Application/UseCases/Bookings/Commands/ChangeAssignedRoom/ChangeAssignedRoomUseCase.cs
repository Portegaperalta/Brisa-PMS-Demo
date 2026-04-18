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
        
        var room = await _roomsRepository.GetById(command.RoomId);

        if (room is null)
            throw new NotFoundException("Room", command.RoomId);

        switch (room.AvailabilityStatus)
        {
            case RoomAvailabilityStatus.Reserved:
                throw new BusinessRuleException("Requested room is already reserved");
            
            case RoomAvailabilityStatus.Occupied:
                throw new BusinessRuleException("Requested room is currently occupied");
            
            case RoomAvailabilityStatus.OutOfService:
                throw new BusinessRuleException("Requested room is currently out of service");
            
            case RoomAvailabilityStatus.Available:
            
            default:
                booking.ChangeAssignedRoom(room.Id);
                room.UpdateAvailabilityStatus(RoomAvailabilityStatus.Reserved);
                break;
        }

        try
        {
            await _roomsRepository.Update(room);
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