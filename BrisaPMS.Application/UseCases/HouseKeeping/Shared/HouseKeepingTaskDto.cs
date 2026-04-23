namespace BrisaPMS.Application.UseCases.HouseKeeping.Shared;

public class HouseKeepingTaskDto
{
    public required Guid Id { get; init; }
    public required Guid RoomId { get; init; }
    public required Guid AssignedBy { get; init; }
    public required Guid AssignedTo { get; init; }
    public required string Type { get; init; }
    public required string Priority { get; init; }
    public required string Status { get; init; }
    public string? Notes { get; init; }
    public required DateTime ExpectedStartAt { get; init; }
    public required DateTime ExpectedEndAt { get; init; }
    public DateTime? ActualStartAt { get; init; }
    public DateTime? ActualEndAt { get; init; }
    public required bool IncidentReported { get; init; }
    public string? IncidentDescription { get; init; }
}