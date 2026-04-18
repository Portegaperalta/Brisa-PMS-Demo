using BrisaPMS.Application.Contracts.Persistence;
using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Commands.MakeGuestVip;

public class MakeGuestVipUseCase : IRequestHandler<MakeGuestVipCommand, bool>
{
  private readonly IGuestsRepository _guestsRepository;
  private readonly IUnitOfWork _unitOfWork;

  public MakeGuestVipUseCase(IGuestsRepository guestsRepository, IUnitOfWork unitOfWork)
  {
    _guestsRepository = guestsRepository;
    _unitOfWork = unitOfWork;
  }

  public async Task<bool> Handle(MakeGuestVipCommand command)
  {
    var guest = await _guestsRepository.GetById(command.GuestId);

    if (guest is null)
      throw new NotFoundException("Guest", command.GuestId);

    guest.EnableVip();

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