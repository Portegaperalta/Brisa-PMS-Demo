using BrisaPMS.Domain.Stays;

namespace BrisaPMS.Application.UseCases.Stays.Shared;

public static class MapperExtension
{
    public static StayDto ToDto(this Stay stay)
    {
        return new StayDto
        {
            Id = stay.Id,
            GuestId = stay.GuestId,
            BookingId = stay.BookingId,
            ActualCheckIn = stay.TimeInterval.ActualCheckIn,
            ActualCheckOut = stay.TimeInterval.ActualCheckOut,
            NightCount = stay.NightCount,
            Status = stay.Status.ToString()
        };
    }
}