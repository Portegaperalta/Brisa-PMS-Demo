using BrisaPMS.Domain.Guests;

namespace BrisaPMS.Application.UseCases.Guests.Shared;

public static class MapperExtension
{
    public static GuestDto ToDto(this Guest guest)
    {
        return new GuestDto
        {
            Id = guest.Id,
            HotelId = guest.HotelId,
            FirstName = guest.FirstName,
            LastName = guest.LastName,
            DocumentType = guest.DocumentType.ToString(),
            DocumentNumber = guest.DocumentNumber,
            Country = guest.Country,
            Rnc = guest.Rnc.Value,
            Email = guest.Email.Value,
            PhoneNumber = guest.PhoneNumber.Value,
            PreferredCurrency = guest.PreferredCurrency.ToString(),
            PreferredLanguage = guest.PreferredLanguage,
            IsVip = guest.IsVip,
            IsBlackListed = guest.IsBlackListed,
            BlackListedReason = guest.BlackListedReason,
            Notes = guest.Notes,
        };
    }
}