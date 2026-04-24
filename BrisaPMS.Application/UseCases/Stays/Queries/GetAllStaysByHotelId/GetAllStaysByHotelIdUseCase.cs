using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Stays.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Stays.Queries.GetAllStaysByHotelId;

public class GetAllStaysByHotelIdUseCase : IRequestHandler<GetAllStaysByHotelIdQuery, List<StayDto>>
{
    private readonly IStaysRepository _staysRepository;
    private readonly IHotelsRepository _hotelsRepository;

    public GetAllStaysByHotelIdUseCase(IStaysRepository staysRepository, IHotelsRepository hotelsRepository)
    {
        _staysRepository = staysRepository;
        _hotelsRepository = hotelsRepository;
    }

    public async Task<List<StayDto>> Handle(GetAllStaysByHotelIdQuery query)
    {
        var hotelExists = await _hotelsRepository.Exists(query.HotelId);

        if (hotelExists is not true)
            throw new NotFoundException("Hotel", query.HotelId);

        var stays = await _staysRepository.GetAllByHotelIdAsync(query.HotelId);
        var staysDtos = stays.Select(s => s.ToDto()).ToList();
        return staysDtos;
    }
}