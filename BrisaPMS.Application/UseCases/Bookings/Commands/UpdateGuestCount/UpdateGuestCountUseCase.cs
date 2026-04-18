using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Bookings;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateGuestCount;

public class UpdateGuestCountUseCase : IRequestHandler<UpdateGuestCountCommand, bool>
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGuestCountUseCase(IBookingsRepository bookingsRepository, IUnitOfWork unitOfWork)
    {
        _bookingsRepository = bookingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateGuestCountCommand command)
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