using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.Booking;
using BrisaPMS.Domain.Bookings;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;
using BrisaPMS.Domain.Shared.Enums;
using BrisaPMS.Domain.Shared.ValueObjects;
using BrisaPMS.Domain.Stays;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.Stays;

public static class StayTestData
{
    public static Stay CreateStay(
        Guid? stayId = null,
        Guid? guestId = null,
        Guid? bookingId = null)
    {
        var stay = new Stay(guestId ?? Guid.NewGuid(), bookingId ?? Guid.NewGuid())
        {
            Id = stayId ?? Guid.NewGuid()
        };

        return stay;
    }

    public static Booking CreateBooking(
        Guid? bookingId = null,
        Guid? hotelId = null,
        Guid? roomId = null,
        Guid? guestId = null)
    {
        return new Booking(
            hotelId ?? Guid.NewGuid(),
            roomId ?? Guid.NewGuid(),
            guestId ?? Guid.NewGuid(),
            BookingSource.Website,
            new GuestCount(2, 1),
            new CheckInOutTimes(
                new DateTime(2026, 4, 20, 15, 0, 0, DateTimeKind.Utc),
                new DateTime(2026, 4, 22, 11, 0, 0, DateTimeKind.Utc)),
            new Money(250.75m, CurrencyCode.USD))
        {
            Id = bookingId ?? Guid.NewGuid()
        };
    }

    public static Room CreateRoom(
        Guid? roomId = null,
        Guid? hotelId = null,
        RoomAvailabilityStatus availabilityStatus = RoomAvailabilityStatus.Available,
        RoomHygieneStatus hygieneStatus = RoomHygieneStatus.Clean)
    {
        return new Room(
            hotelId ?? Guid.NewGuid(),
            "101",
            1,
            availabilityStatus,
            hygieneStatus,
            CreateRoomType())
        {
            Id = roomId ?? Guid.NewGuid()
        };
    }

    public static RoomType CreateRoomType()
    {
        return new RoomType(
            "Deluxe Suite",
            new RoomBaseRate(0.25m),
            new RoomBed(BedType.Double, 1),
            new OccupancyPolicy(2, 1),
            "Ocean view suite");
    }
}
