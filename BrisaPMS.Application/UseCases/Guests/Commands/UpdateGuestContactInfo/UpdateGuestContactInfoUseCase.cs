using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;
using BrisaPMS.Domain.Shared.ValueObjects;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestContactInfo;

public class UpdateGuestContactInfoUseCase : IRequestHandler<UpdateGuestContactInfoCommand, bool>
{
    private readonly IGuestsRepository _guestsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGuestContactInfoUseCase(IGuestsRepository guestsRepository, IUnitOfWork unitOfWork)
    {
        _guestsRepository = guestsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateGuestContactInfoCommand command)
    {
        var guest = await _guestsRepository.GetById(command.GuestId);

        if (guest is null)
            throw new NotFoundException("Guest", command.GuestId);
        
        var newEmail = new Email(command.Email);
        var newPhoneNumber = new PhoneNumber(command.PhoneNumber);
        
        guest.ChangeEmail(newEmail);
        guest.ChangePhoneNumber(newPhoneNumber);

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