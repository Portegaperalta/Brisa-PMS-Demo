using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.MarkAsNoShow;

public class MarkAsNoShowUseCase : IRequestHandler<MarkAsNoShowCommand, bool>
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkAsNoShowUseCase(IBookingsRepository bookingsRepository, IRoomsRepository roomsRepository,
        IUnitOfWork unitOfWork)
    {
        _bookingsRepository = bookingsRepository;
        _roomsRepository = roomsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(MarkAsNoShowCommand command)
    {
        var booking = await _bookingsRepository.GetById(command.BookingId);
        
        if (booking is null)
            throw new NotFoundException("Booking", command.BookingId);
        
        var room = await _roomsRepository.GetById(booking.RoomId);
        
        booking.SetAsNoShow();
        room!.UpdateAvailabilityStatus(RoomAvailabilityStatus.Available);

        try
        {
            await _bookingsRepository.Update(booking);
            await _roomsRepository.Update(room);
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