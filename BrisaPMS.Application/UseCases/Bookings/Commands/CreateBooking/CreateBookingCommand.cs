using BrisaPMS.Application.UseCases.Bookings.Queries.Shared;
using BrisaPMS.Application.Utilities.Mediator;

namespace BrisaPMS.Application.UseCases.Bookings.Commands.CreateBooking;

public class CreateBookingCommand : IRequest<BookingDto>
{
    public required Guid HotelId { get; set; }
    public required Guid RoomId { get; set; }
    public required Guid GuestId { get; set; }
    public required string Source { get; set; }
    public required int NumberOfAdults { get; set; }
    public required int NumberOfChildren { get; set; }
    public required DateTime CheckInTime { get; set; }
    public required DateTime CheckOutTime { get; set; }
    public string? SpecialRequests { get; set; }
    public required decimal TotalPrice { get; set; }
    public Guid? DiscountId { get; set; }
}