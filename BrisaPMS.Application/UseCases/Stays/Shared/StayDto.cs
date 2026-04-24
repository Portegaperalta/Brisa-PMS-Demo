namespace BrisaPMS.Application.UseCases.Stays.Shared;

public class StayDto 
{
    public required Guid Id { get; init; }
    public required Guid GuestId { get; init; }
    public required Guid BookingId { get; init; }
    public required DateTime ActualCheckIn { get; init; }
    public DateTime? ActualCheckOut { get; init; }
    public required int NightCount { get; init; }
    public required string Status { get; init; }
}