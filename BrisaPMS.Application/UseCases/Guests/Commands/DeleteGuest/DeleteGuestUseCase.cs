using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.DeleteGuest;

public class DeleteGuestUseCase : IRequestHandler<DeleteGuestCommand, bool>
{
    private readonly IGuestsRepository _guestsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteGuestUseCase(IGuestsRepository guestsRepository, IUnitOfWork unitOfWork)
    {
        _guestsRepository = guestsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteGuestCommand command)
    {
        var guest = await _guestsRepository.GetById(command.Id) ??
                    throw new NotFoundException("Guest", command.Id);

        try
        {
            await _guestsRepository.Delete(guest);
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