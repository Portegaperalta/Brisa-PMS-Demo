using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.UpdateGuestGeneralInfo;

public class UpdateGuestGeneralInfoUseCase : IRequestHandler<UpdateGuestGeneralInfoCommand, bool>
{
    private readonly IGuestsRepository _guestsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGuestGeneralInfoUseCase(IGuestsRepository guestsRepository, IUnitOfWork unitOfWork)
    {
        _guestsRepository = guestsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(UpdateGuestGeneralInfoCommand command)
    {
        var guest = await _guestsRepository.GetById(command.GuestId);

        if (guest is null)
            throw new NotFoundException("Guest", command.GuestId);
        
        guest.ChangeFirstName(command.FirstName);
        guest.ChangeLastName(command.LastName);
        
        if (string.IsNullOrWhiteSpace(command.Country) is not true)
            guest.ChangeCountry(command.Country);
        
        if (string.IsNullOrWhiteSpace(command.PreferredLanguage) is not true)
            guest.ChangePreferredLanguage(command.PreferredLanguage);

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