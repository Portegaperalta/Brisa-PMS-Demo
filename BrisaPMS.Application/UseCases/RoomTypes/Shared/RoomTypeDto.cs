namespace BrisaPMS.Application.UseCases.RoomTypes.Shared;

public class RoomTypeDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public required decimal BaseRate { get; init; }
    public required int NumberOfBeds { get; init; }
    public required string BedType { get; init; }
    public required int MaxOccupancyAdults { get; init; }
    public required int MaxOccupancyChildren { get; init; }
}