using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.UpdateSpecialRequests;

public class UpdateSpecialRequestsUseCase : IRequestHandler<UpdateSpecialRequestsCommand, bool>
{
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSpecialRequestsUseCase(IBookingsRepository bookingsRepository, IUnitOfWork unitOfWork)
    {
        _bookingsRepository = bookingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateSpecialRequestsCommand command)
    {
        var booking = await _bookingsRepository.GetById(command.BookingId);
        
        if (booking is null)
            throw new NotFoundException("Booking", command.BookingId);
        
        booking.UpdateSpecialRequests(command.SpecialRequests);
        
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