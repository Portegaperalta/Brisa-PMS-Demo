using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Guests.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Guests.Queries.GetAllGuestsByHotelId;

public class GetAllGuestsByHotelIdUseCase : IRequestHandler<GetAllGuestsByHotelIdQuery, List<GuestDto>>
{
    private readonly IGuestsRepository _guestsRepository;
    private readonly IHotelsRepository _hotelsRepository;

    public GetAllGuestsByHotelIdUseCase(IGuestsRepository guestsRepository,  IHotelsRepository hotelsRepository)
    {
        _guestsRepository = guestsRepository;
        _hotelsRepository = hotelsRepository;
    }

    public async Task<List<GuestDto>> Handle(GetAllGuestsByHotelIdQuery query)
    {
        var hotelExists = await _hotelsRepository.Exists(query.HotelId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", query.HotelId);

        var guests = await _guestsRepository.GetAllByHotelIdAsync(query.HotelId);
        var guestsDtos = guests.Select(g => g.ToDto()).ToList();
        return guestsDtos;
    }
}