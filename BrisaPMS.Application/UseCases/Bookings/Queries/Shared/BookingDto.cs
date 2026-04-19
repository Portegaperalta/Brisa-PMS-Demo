namespace BrisaPMS.Application.UseCases.Bookings.Queries.Shared;

public class BookingDto
{
    public required Guid Id { get; init; }
    public required Guid HotelId { get; init; }
    public required Guid RoomId { get; init; }
    public required Guid GuestId { get; init; }
    public required string Source { get; init; }
    public required int NumberOfAdults { get; init; }
    public required int NumberOfChildren { get; init; }
    public required DateTime CheckInTime { get; init; }
    public required DateTime CheckOutTime { get; init; }
    public string? SpecialRequests { get; init; }
    public required string Status { get; init; }
    public string? CancellationReason { get; init; }
    public required decimal TotalPrice { get; init; }
    public Guid? DiscountId { get; init; }
}