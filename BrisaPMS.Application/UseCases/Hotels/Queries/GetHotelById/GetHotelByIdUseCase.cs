using BrisaPMS.Application.Contracts.Repositories;
using BrisaPMS.Application.Exceptions;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Hotels.Queries.GetHotelById;

public class GetHotelByIdUseCase : IRequestHandler<GetHotelByIdQuery, HotelDto>
{
    private IHotelsRepository _repository;

    public GetHotelByIdUseCase(IHotelsRepository repository) { _repository = repository; }
    
    public async Task<HotelDto> Handle(GetHotelByIdQuery request)
    {
        var hotel = await _repository.GetById(request.HotelId);

        if (hotel is null)
            throw new NotFoundException("Hotel", request.HotelId);

        var dto = new HotelDto()
        {
            Id = hotel.Id,
            LegalName =  hotel.LegalName,
            CommercialName = hotel.CommercialName,
            LogoUrl = hotel.LogoUrl!.Value,
            BusinessEmail = hotel.BusinessEmail.Value,
            BusinessPhoneNumber = hotel.BusinessPhoneNumber.Value,
            Address1 = hotel.Address.Address1,
            Address2 = hotel.Address.Address2,
            City = hotel.Address.City,
            Province = hotel.Address.Province,
            ZipCode = hotel.Address.ZipCode,
            CheckInTime = hotel.CheckOutPolicy.CheckInTime,
            CheckOutTime = hotel.CheckOutPolicy.CheckOutTime,
            DefaultCurrencyCode = hotel.DefaultCurrencyCode.ToString(),
            ItbisRate = hotel.ItbisRate.Rate,
            ServiceChargeRate = hotel.ServiceChargeRate.Rate,
            IsActive =  hotel.IsActive,
        };
        
        return dto;
    }
}