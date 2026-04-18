namespace BrisaPMS.Application.UseCases.Amenities.Shared;

public class AmenityDto
{
    public required Guid Id { get; init; }
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required bool IsActive { get; init; }
}