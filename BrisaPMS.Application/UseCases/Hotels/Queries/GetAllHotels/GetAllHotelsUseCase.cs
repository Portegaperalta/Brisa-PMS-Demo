using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.UseCases.Hotels.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Queries.GetAllHotels;

public class GetAllHotelsUseCase : IRequestHandler<GetAllHotelsQuery, List<HotelDto>>
{
    private readonly IHotelsRepository _repository;
    
    public GetAllHotelsUseCase(IHotelsRepository repository) { _repository = repository; }

    public async Task<List<HotelDto>> Handle(GetAllHotelsQuery request)
    {
        var hotel = await _repository.GetAll();
        var hotelsDtos = hotel.Select(h => h.ToDto()).ToList();
        
        return hotelsDtos;
    }
}