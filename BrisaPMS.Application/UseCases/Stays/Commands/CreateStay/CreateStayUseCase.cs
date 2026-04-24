using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Booking;
using BrisaPMS.Domain.Shared.Exceptions;
using BrisaPMS.Domain.Stays;

namespace BrisaPMS.Application.UseCases.Stays.Commands.CreateStay;

public class CreateStayUseCase : IRequestHandler<CreateStayCommand, Guid>
{
    private readonly IStaysRepository _staysRepository;
    private readonly IGuestsRepository _guestsRepository;
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateStayUseCase(IStaysRepository staysRepository, IGuestsRepository guestsRepository, 
        IBookingsRepository bookingsRepository,IUnitOfWork unitOfWork)
    {
        _staysRepository = staysRepository;
        _guestsRepository = guestsRepository;
        _bookingsRepository = bookingsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateStayCommand command)
    {
        var guestExists = await _guestsRepository.Exists(command.GuestId);

        if (guestExists is not true)
            throw new NotFoundException("Guest", command.GuestId);

        var bookingExists = await _bookingsRepository.Exists(command.BookingId);

        if (bookingExists is not true)
            throw new NotFoundException("Booking", command.BookingId);
        
        var bookingStatus = await _bookingsRepository.GetBookingStatusAsync(command.BookingId);
        
        if (bookingStatus is "Complete" or "Cancelled")
            throw new BusinessRuleException("Can't create stay if booking is already completed or cancelled");

        var stay = new Stay(command.GuestId, command.BookingId);

        try
        {
            await _staysRepository.Create(stay);
            await _unitOfWork.Persist();
            return stay.Id;
        }
        catch (Exception)
        {
            await _unitOfWork.Revert();
            throw;
        }
    }
}