using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestRnc;

public class UpdateGuestRncUseCase : IRequestHandler<UpdateGuestRncCommand, bool>
{
    private readonly IGuestsRepository _guestsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGuestRncUseCase(IGuestsRepository guestsRepository, IUnitOfWork unitOfWork)
    {
        _guestsRepository = guestsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateGuestRncCommand command)
    {
        var guest = await _guestsRepository.GetById(command.GuestId);

        if (guest is null)
            throw new NotFoundException("Guest", command.GuestId);

        var rnc = new Rnc(command.Rnc);
        guest.ChangeRnc(rnc);

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