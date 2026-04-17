using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.UseCases.Hotels.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;

public class GetGuestByHotelIdUseCase : IRequestHandler<GetHotelByIdQuery, HotelDto>
{
    private readonly IHotelsRepository _repository;

    public GetGuestByHotelIdUseCase(IHotelsRepository repository) { _repository = repository; }
    
    public async Task<HotelDto> Handle(GetHotelByIdQuery request)
    {
        var hotel = await _repository.GetById(request.HotelId);

        if (hotel is null)
            throw new NotFoundException("Hotel", request.HotelId);

        return hotel.ToDto();
    }
}