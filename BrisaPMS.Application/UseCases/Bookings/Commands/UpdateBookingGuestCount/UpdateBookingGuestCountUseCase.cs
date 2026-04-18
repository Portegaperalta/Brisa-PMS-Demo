using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Bookings;
using FluentValidation;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateBookingGuestCount;

public class UpdateBookingGuestCountUseCase : IRequestHandler<UpdateBookingGuestCountCommand, bool>
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateBookingGuestCountUseCase(IBookingsRepository bookingsRepository, IUnitOfWork unitOfWork)
    {
        _bookingsRepository = bookingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateBookingGuestCountCommand command)
    {
        var booking = await _bookingsRepository.GetById(command.BookingId);

        if (booking is null)
            throw new NotFoundException("Booking", command.BookingId);

        var newGuestCount = new GuestCount(command.NumberOfAdults, command.NumberOfChildren);
        booking.UpdateGuestCount(newGuestCount);

        try
        {
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