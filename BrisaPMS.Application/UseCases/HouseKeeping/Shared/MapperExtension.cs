using BrisaPMS.Domain.HouseKeeping;

namespace BrisaPMS.Application.UseCases.HouseKeeping.Shared;

public static class MapperExtension
{
    public static HouseKeepingTaskDto ToDto(this HouseKeepingTask houseKeepingTask)
    {
        return new HouseKeepingTaskDto
        {
            Id = houseKeepingTask.Id,
            RoomId = houseKeepingTask.RoomId,
            AssignedBy = houseKeepingTask.AssignedBy,
            AssignedTo = houseKeepingTask.AssignedTo,
            Type = houseKeepingTask.Type.ToString(),
            Priority = houseKeepingTask.Priority.ToString(),
            Status = houseKeepingTask.Status.ToString(),
            Notes = houseKeepingTask.Notes,
            ExpectedStartAt = houseKeepingTask.Deadline.ExpectedStartAt,
            ExpectedEndAt = houseKeepingTask.Deadline.ExpectedEndAt,
            ActualStartAt = houseKeepingTask.ActualTimeInterval?.ActualStartAt,
            ActualEndAt = houseKeepingTask.ActualTimeInterval?.ActualEndAt,
            IncidentReported = houseKeepingTask.IncidentReported,
            IncidentDescription = houseKeepingTask.IncidentDescription
        };
    }
}