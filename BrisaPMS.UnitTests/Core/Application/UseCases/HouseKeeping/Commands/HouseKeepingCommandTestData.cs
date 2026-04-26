using BrisaPMS.Domain.Billing;
using BrisaPMS.Domain.HouseKeeping;
using BrisaPMS.Domain.Rooms;
using BrisaPMS.Domain.RoomTypes;

namespace BrisaPMS.UnitTests.Core.Application.UseCases.HouseKeeping.Commands;

public static class HouseKeepingCommandTestData
{
    public static TaskDeadline CreateTaskDeadline(
        DateTime? expectedStartAt = null,
        DateTime? expectedEndAt = null)
    {
        return new TaskDeadline(
            expectedStartAt ?? new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
            expectedEndAt ?? new DateTime(2026, 4, 1, 11, 0, 0, DateTimeKind.Utc));
    }

    public static HouseKeepingTask CreateHouseKeepingTask(
        Guid? hotelId = null,
        Guid? roomId = null,
        Guid? assignedTo = null,
        Guid? assignedBy = null,
        HouseKeepingTaskType type = HouseKeepingTaskType.Cleaning,
        TaskPriority priority = TaskPriority.High,
        string? notes = "Clean room before next guest arrival",
        bool startActualTimeInterval = false,
        string? incidentDescription = null)
    {
        var houseKeepingTask = new HouseKeepingTask(
            hotelId ?? Guid.NewGuid(),
            roomId ?? Guid.NewGuid(),
            assignedTo ?? Guid.NewGuid(),
            assignedBy ?? Guid.NewGuid(),
            type,
            priority,
            CreateTaskDeadline(),
            notes);

        if (startActualTimeInterval)
        {
            houseKeepingTask.StartActualTimeInterval();
        }

        if (!string.IsNullOrWhiteSpace(incidentDescription))
        {
            houseKeepingTask.ReportIncident(incidentDescription);
        }

        return houseKeepingTask;
    }

    public static Room CreateRoom(
        Guid? roomId = null,
        Guid? hotelId = null,
        RoomAvailabilityStatus availabilityStatus = RoomAvailabilityStatus.Available,
        RoomHygieneStatus hygieneStatus = RoomHygieneStatus.Dirty)
    {
        return new Room(
            Guid.NewGuid(),
            hotelId ?? Guid.NewGuid(),
            "101",
            1,
            availabilityStatus,
            hygieneStatus)
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
