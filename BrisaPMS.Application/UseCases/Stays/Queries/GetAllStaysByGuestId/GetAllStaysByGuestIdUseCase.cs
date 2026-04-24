using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByGuestId;

public class GetAllStaysByGuestIdUseCase : IRequestHandler<GetAllStaysByGuestIdQuery, List<StayDto>>
{
    private readonly IStaysRepository _staysRepository;
    private readonly IGuestsRepository _guestsRepository;

    public GetAllStaysByGuestIdUseCase(IStaysRepository staysRepository, IGuestsRepository guestsRepository)
    {
        _staysRepository = staysRepository;
        _guestsRepository = guestsRepository;
    }

    public async Task<List<StayDto>> Handle(GetAllStaysByGuestIdQuery query)
    {
        var guestExists = await _guestsRepository.Exists(query.GuestId);

        if (guestExists is not true)
            throw new NotFoundException("Guest", query.GuestId);

        var stays = await _staysRepository.GetAllByGuestIdAsync(query.GuestId);
        var stayDtos = stays.Select(s => s.ToDto()).ToList();
        return stayDtos;
    }
}