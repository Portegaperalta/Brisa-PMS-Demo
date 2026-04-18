using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.CreateBooking;

public class CreateBookingUseCase : IRequestHandler<CreateBookingCommand, Guid>
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IHotelsRepository _hotelsRepository;
    private readonly IGuestsRepository _guestsRepository;
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBookingUseCase(IBookingsRepository bookingsRepository, IHotelsRepository hotelsRepository,
        IGuestsRepository guestsRepository ,IRoomsRepository roomsRepository, IUnitOfWork unitOfWork)
    {
        _bookingsRepository = bookingsRepository;
        _hotelsRepository = hotelsRepository;
        _guestsRepository = guestsRepository;
        _roomsRepository = roomsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateBookingCommand command)
    {
        var hotelExists = await _hotelsRepository.Exists(command.HotelId);
        var guestExists = await _guestsRepository.Exists(command.GuestId);
        var room = await _roomsRepository.GetById(command.RoomId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", command.HotelId);
        
        if (guestExists is not true)
            throw new NotFoundException("Guest", command.GuestId);
        
        if (room is null)
            throw new NotFoundException("Room", command.RoomId);

        switch (room.AvailabilityStatus)
        {
            case RoomAvailabilityStatus.Reserved:
                throw new BusinessRuleException("Room is reserved, unable to create booking");
            
            case RoomAvailabilityStatus.Occupied:
                throw new BusinessRuleException("Room is occupied, unable to create booking");
            
            case RoomAvailabilityStatus.OutOfService:
                throw new BusinessRuleException("Room is out of service, unable to create booking");
            
            case RoomAvailabilityStatus.Available:
                room.UpdateAvailabilityStatus(RoomAvailabilityStatus.Reserved);
                break;
            
            default:
                throw new BusinessRuleException("Availability status not supported");
        }

        var guestCount = new GuestCount(command.NumberOfAdults,  command.NumberOfChildren);
        var checkInOutTimes = new CheckInOutTimes(command.CheckInTime, command.CheckOutTime);
        var totalPrice = new Money(command.TotalPrice);
        
        var booking = new Booking
        (
            command.HotelId,
            command.RoomId,
            command.GuestId,
            command.Source,
            guestCount,
            checkInOutTimes,
            totalPrice,
            command.SpecialRequests,
            command.DiscountId
        );

        try
        {
            await _roomsRepository.Update(room);
            await _bookingsRepository.Create(booking);
            await _unitOfWork.Persist();
            return booking.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}