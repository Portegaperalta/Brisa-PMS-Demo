using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Queries.GetGuestByHotelId;

public class GetGuestByHotelIdUseCase : IRequestHandler<GetGuestByHotelIdQuery, GuestDto?>
{
    private readonly IGuestsRepository  _guestsRepository;
    private readonly IHotelsRepository _hotelsRepository;

    public GetGuestByHotelIdUseCase(IGuestsRepository guestsRepository, IHotelsRepository hotelsRepository)
    {
        _guestsRepository = guestsRepository;
        _hotelsRepository = hotelsRepository;
    }

    public async Task<GuestDto?> Handle(GetGuestByHotelIdQuery query)
    {
        var hotelExists = await _hotelsRepository.Exists(query.HotelId);
        
        if (hotelExists is not true)
            throw new NotFoundException("Hotel", query.HotelId);

        var guest = await _guestsRepository.GetByHotelIdAsync(query.HotelId);

        if (guest is null)
            return null;

        var guestDto = guest.ToDto();
        return guestDto;
    }
}