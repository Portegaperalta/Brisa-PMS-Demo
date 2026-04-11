namespace BrisaPMS.Application.UseCases.Rooms.Shared;

public class RoomDto
{
    public required Guid Id { get; init; }
    public required Guid HotelId { get; init;}
    public required string Number { get; init; }
    public required int Floor { get; init; }
    public required string AvailabilityStatus { get; init; }
    public required string HygieneStatus { get; init; }
    public DateTime? LastCleanedAt { get; init; }
    public Guid? LastCleanedBy { get; init; }
    public required bool NeedsRestocking { get; init; }
}