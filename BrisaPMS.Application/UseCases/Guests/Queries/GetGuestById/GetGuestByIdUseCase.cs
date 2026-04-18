using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Queries.GetGuestById;

public class GetGuestByIdUseCase : IRequestHandler<GetGuestByIdQuery, GuestDto>
{
    private readonly IGuestsRepository  _guestsRepository;

    public GetGuestByIdUseCase(IGuestsRepository guestsRepository) { _guestsRepository = guestsRepository; }

    public async Task<GuestDto> Handle(GetGuestByIdQuery query)
    {
        var guest = await _guestsRepository.GetById(query.GuestId);

        if (guest is null)
            throw new NotFoundException("Guest", query.GuestId);

        var guestDto = guest.ToDto();
        return guestDto;
    }
}