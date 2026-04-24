using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Rooms;

namespace BrisaPMS.Application.UseCases.Stays.Commands.CompleteStay;

public class CompleteStayUseCase : IRequestHandler<CompleteStayCommand, bool>
{
    private readonly IStaysRepository _staysRepository;
    private readonly IBookingsRepository _bookingsRepository;
    private readonly IRoomsRepository _roomsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CompleteStayUseCase(IStaysRepository staysRepository, IBookingsRepository bookingsRepository,
        IRoomsRepository roomsRepository,IUnitOfWork unitOfWork)
    {
        _staysRepository = staysRepository;
        _bookingsRepository = bookingsRepository;
        _roomsRepository = roomsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(CompleteStayCommand command)
    {
        var stay = await _staysRepository.GetById(command.StayId);
        
        if (stay is null)
            throw new NotFoundException("Stay", command.StayId);
        
        var booking = await _bookingsRepository.GetById(stay.BookingId);
        var room = await _roomsRepository.GetById(booking!.RoomId);
        
        stay.SetAsComplete();
        booking.SetAsCompleted();
        room!.UpdateHygieneStatus(RoomHygieneStatus.Dirty);

        try
        {
            await _staysRepository.Update(stay);
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