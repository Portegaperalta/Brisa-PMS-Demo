using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Guest;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestDocumentation;

public class UpdateGuestDocumentationUseCase : IRequestHandler<UpdateGuestDocumentationCommand, bool>
{
    private readonly IGuestsRepository _guestsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGuestDocumentationUseCase(IGuestsRepository guestsRepository, IUnitOfWork unitOfWork)
    {
        _guestsRepository = guestsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateGuestDocumentationCommand command)
    {
        var guest = await _guestsRepository.GetById(command.GuestId);

        if (guest is null)
            throw new NotFoundException("Guest", command.GuestId);

        var newDocumentType = Enum.Parse<GuestDocumentType>(command.DocumentType);
        
        guest.ChangeDocumentType(newDocumentType);
        guest.ChangeDocumentNumber(command.DocumentNumber);

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