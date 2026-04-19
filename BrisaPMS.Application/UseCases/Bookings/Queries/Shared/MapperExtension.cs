using BrisaPMS.Domain.Bookings;

namespace BrisaPMS.Application.UseCases.Bookings.Queries.Shared;

public static class MapperExtension
{
    public static BookingDto ToDto(this Booking booking)
    {
        return new BookingDto
        {
            Id = booking.Id,
            HotelId = booking.HotelId,
            RoomId = booking.RoomId,
            GuestId = booking.GuestId,
            Source = booking.Source.ToString(),
            NumberOfAdults = booking.GuestCount.NumberOfAdults,
            NumberOfChildren = booking.GuestCount.NumberOfChildren,
            CheckInTime = booking.CheckInOutTimes.CheckInTime,
            CheckOutTime = booking.CheckInOutTimes.CheckOutTime,
            SpecialRequests = booking.SpecialRequests,
            Status = booking.Status.ToString(),
            CancellationReason = booking.CancellationReason,
            TotalPrice = booking.TotalPrice.Amount,
            DiscountId = booking.DiscountId,
        };
    }
}