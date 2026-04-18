using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.BlacklistGuest;

public class BlacklistGuestUseCase : IRequestHandler<BlacklistGuestCommand, bool>
{
    private readonly IGuestsRepository _guestsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public BlacklistGuestUseCase(IGuestsRepository guestsRepository, IUnitOfWork unitOfWork)
    {
        _guestsRepository = guestsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(BlacklistGuestCommand command)
    {
        var guest = await _guestsRepository.GetById(command.GuestId);
        
        if (guest is null)
            throw new NotFoundException("Guest", command.GuestId);
        
        guest.BlackList(command.BlacklistedReason);

        try
        {
            await _guestsRepository.Update(guest);
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