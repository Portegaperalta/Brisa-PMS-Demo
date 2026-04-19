using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateCheckInOutTimes;

public class UpdateCheckInOutTimesUseCase : IRequestHandler<UpdateCheckInOutTimesCommand, bool>
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCheckInOutTimesUseCase(IBookingsRepository bookingsRepository, IUnitOfWork unitOfWork)
    {
        _bookingsRepository = bookingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateCheckInOutTimesCommand command)
    {
        var booking = await _bookingsRepository.GetById(command.BookingId);

        if (booking is null)
            throw new NotFoundException("Booking", command.BookingId);

        var newCheckInOutTimes = new CheckInOutTimes(command.CheckInTime, command.CheckOutTime);
        booking.UpdateCheckInOutTimes(newCheckInOutTimes);

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