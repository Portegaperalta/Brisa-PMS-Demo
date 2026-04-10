using BrisaPMS.Domain.Hotels;

namespace BrisaPMS.Application.UseCases.Hotels.Shared;

public static class MapperExtension
{
    public static HotelDto ToDto(this Hotel hotel)
    {
        return new HotelDto()
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
    }
}