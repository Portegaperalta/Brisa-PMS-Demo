using BrisaPMS.Domain.HouseKeeping;
using BrisaPMS.Domain.Shared.Exceptions;
using FluentAssertions;

namespace BrisaPMS.UnitTests.Core.Domain.Entities;

public class HouseKeepingTaskTests
{
    [Fact]
    public void Constructor_ShouldCreateHouseKeepingTask_WhenValuesAreValid()
    {
        // Arrange
        var roomId = Guid.NewGuid();
        var assignedTo = Guid.NewGuid();
        var assignedBy = Guid.NewGuid();
        var expectedTimeInterval = CreateExpectedTimeInterval();
        const string notes = "Clean room before next guest arrival";

        // Act
        var result = new HouseKeepingTask(
            roomId,
            assignedTo,
            assignedBy,
            HouseKeepingTaskType.Cleaning,
            TaskPriority.High,
            expectedTimeInterval,
            notes);

        // Assert
        result.Id.Should().NotBe(Guid.Empty);
        result.RoomId.Should().Be(roomId);
        result.AssignedTo.Should().Be(assignedTo);
        result.AssignedBy.Should().Be(assignedBy);
        result.Type.Should().Be(HouseKeepingTaskType.Cleaning);
        result.Priority.Should().Be(TaskPriority.High);
        result.Status.Should().Be(HouseKeepingTaskStatus.Pending);
        result.Notes.Should().Be(notes);
        result.ExpectedTimeInterval.Should().Be(expectedTimeInterval);
        result.ActualTimeInterval.Should().BeNull();
        result.IncidentReported.Should().BeFalse();
        result.IncidentDescription.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldAllowNullNotes()
    {
        // Arrange
        var expectedTimeInterval = CreateExpectedTimeInterval();

        // Act
        var result = new HouseKeepingTask(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            HouseKeepingTaskType.Restocking,
            TaskPriority.Medium,
            expectedTimeInterval);

        // Assert
        result.Notes.Should().BeNull();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenRoomIdIsEmpty()
    {
        // Arrange
        var roomId = Guid.Empty;

        // Act
        Action act = () => _ = new HouseKeepingTask(
            roomId,
            Guid.NewGuid(),
            Guid.NewGuid(),
            HouseKeepingTaskType.Cleaning,
            TaskPriority.High,
            CreateExpectedTimeInterval());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenAssignedToIsEmpty()
    {
        // Arrange
        var assignedTo = Guid.Empty;

        // Act
        Action act = () => _ = new HouseKeepingTask(
            Guid.NewGuid(),
            assignedTo,
            Guid.NewGuid(),
            HouseKeepingTaskType.Cleaning,
            TaskPriority.High,
            CreateExpectedTimeInterval());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowEmptyRequiredFieldException_WhenAssignedByIsEmpty()
    {
        // Arrange
        var assignedBy = Guid.Empty;

        // Act
        Action act = () => _ = new HouseKeepingTask(
            Guid.NewGuid(),
            Guid.NewGuid(),
            assignedBy,
            HouseKeepingTaskType.Cleaning,
            TaskPriority.High,
            CreateExpectedTimeInterval());

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenTypeIsInvalid()
    {
        // Arrange
        var invalidType = (HouseKeepingTaskType)999;

        // Act
        Action act = () => _ = new HouseKeepingTask(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            invalidType,
            TaskPriority.High,
            CreateExpectedTimeInterval());

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void Constructor_ShouldThrowBusinessRuleException_WhenPriorityIsInvalid()
    {
        // Arrange
        var invalidPriority = (TaskPriority)999;

        // Act
        Action act = () => _ = new HouseKeepingTask(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            HouseKeepingTaskType.Cleaning,
            invalidPriority,
            CreateExpectedTimeInterval());

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangeAssignedTo_ShouldUpdateAssignedTo_WhenValueIsValid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        var newAssignedTo = Guid.NewGuid();

        // Act
        houseKeepingTask.ChangeAssignedTo(newAssignedTo);

        // Assert
        houseKeepingTask.AssignedTo.Should().Be(newAssignedTo);
    }

    [Fact]
    public void ChangeAssignedTo_ShouldThrowEmptyRequiredFieldException_WhenValueIsEmpty()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        Action act = () => houseKeepingTask.ChangeAssignedTo(Guid.Empty);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Theory]
    [InlineData(HouseKeepingTaskStatus.Completed)]
    [InlineData(HouseKeepingTaskStatus.Cancelled)]
    public void ChangeAssignedTo_ShouldThrowBusinessRuleException_WhenStatusDoesNotAllowChanges(HouseKeepingTaskStatus status)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.UpdatedStatus(status);

        // Act
        Action act = () => houseKeepingTask.ChangeAssignedTo(Guid.NewGuid());

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangeTaskType_ShouldUpdateTaskType_WhenValueIsValid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        houseKeepingTask.ChangeTaskType(HouseKeepingTaskType.Inspection);

        // Assert
        houseKeepingTask.Type.Should().Be(HouseKeepingTaskType.Inspection);
    }

    [Fact]
    public void ChangeTaskType_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        var invalidType = (HouseKeepingTaskType)999;

        // Act
        Action act = () => houseKeepingTask.ChangeTaskType(invalidType);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Theory]
    [InlineData(HouseKeepingTaskStatus.Completed)]
    [InlineData(HouseKeepingTaskStatus.Cancelled)]
    public void ChangeTaskType_ShouldThrowBusinessRuleException_WhenStatusDoesNotAllowChanges(HouseKeepingTaskStatus status)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.UpdatedStatus(status);

        // Act
        Action act = () => houseKeepingTask.ChangeTaskType(HouseKeepingTaskType.DeepCleaning);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ChangePriority_ShouldUpdatePriority_WhenValueIsValid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        houseKeepingTask.ChangePriority(TaskPriority.Urgent);

        // Assert
        houseKeepingTask.Priority.Should().Be(TaskPriority.Urgent);
    }

    [Fact]
    public void ChangePriority_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        var invalidPriority = (TaskPriority)999;

        // Act
        Action act = () => houseKeepingTask.ChangePriority(invalidPriority);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Theory]
    [InlineData(HouseKeepingTaskStatus.Completed)]
    [InlineData(HouseKeepingTaskStatus.Cancelled)]
    public void ChangePriority_ShouldThrowBusinessRuleException_WhenStatusDoesNotAllowChanges(HouseKeepingTaskStatus status)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.UpdatedStatus(status);

        // Act
        Action act = () => houseKeepingTask.ChangePriority(TaskPriority.Low);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdatedStatus_ShouldUpdateStatus_WhenValueIsValid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        houseKeepingTask.UpdatedStatus(HouseKeepingTaskStatus.InProgress);

        // Assert
        houseKeepingTask.Status.Should().Be(HouseKeepingTaskStatus.InProgress);
    }

    [Fact]
    public void UpdatedStatus_ShouldThrowBusinessRuleException_WhenValueIsInvalid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        var invalidStatus = (HouseKeepingTaskStatus)999;

        // Act
        Action act = () => houseKeepingTask.UpdatedStatus(invalidStatus);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Theory]
    [InlineData(HouseKeepingTaskStatus.Completed)]
    [InlineData(HouseKeepingTaskStatus.Cancelled)]
    public void UpdatedStatus_ShouldThrowBusinessRuleException_WhenCurrentStatusDoesNotAllowChanges(HouseKeepingTaskStatus status)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.UpdatedStatus(status);

        // Act
        Action act = () => houseKeepingTask.UpdatedStatus(HouseKeepingTaskStatus.InProgress);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void UpdatedNotes_ShouldUpdateNotes_WhenValueIsProvided()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        houseKeepingTask.UpdatedNotes("Replace towels and amenities");

        // Assert
        houseKeepingTask.Notes.Should().Be("Replace towels and amenities");
    }

    [Fact]
    public void UpdatedNotes_ShouldAllowNullValue()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        houseKeepingTask.UpdatedNotes(null);

        // Assert
        houseKeepingTask.Notes.Should().BeNull();
    }

    [Fact]
    public void ChangeExpectedTimeInterval_ShouldUpdateExpectedTimeInterval_WhenValueIsValid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        var newExpectedTimeInterval = new TaskExpectedTimeInterval(
            new DateTime(2026, 4, 2, 14, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 2, 15, 0, 0, DateTimeKind.Utc));

        // Act
        houseKeepingTask.ChangeExpectedTimeInterval(newExpectedTimeInterval);

        // Assert
        houseKeepingTask.ExpectedTimeInterval.Should().Be(newExpectedTimeInterval);
    }

    [Theory]
    [InlineData(HouseKeepingTaskStatus.Completed)]
    [InlineData(HouseKeepingTaskStatus.Cancelled)]
    public void ChangeExpectedTimeInterval_ShouldThrowBusinessRuleException_WhenStatusDoesNotAllowChanges(HouseKeepingTaskStatus status)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.UpdatedStatus(status);
        var newExpectedTimeInterval = new TaskExpectedTimeInterval(
            new DateTime(2026, 4, 2, 14, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 2, 15, 0, 0, DateTimeKind.Utc));

        // Act
        Action act = () => houseKeepingTask.ChangeExpectedTimeInterval(newExpectedTimeInterval);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void StartActualTimeInterval_ShouldCreateActualTimeInterval_WhenStatusAllowsIt()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        houseKeepingTask.StartActualTimeInterval();

        // Assert
        houseKeepingTask.ActualTimeInterval.Should().NotBeNull();
        houseKeepingTask.ActualTimeInterval!.ActualStartAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        houseKeepingTask.ActualTimeInterval.ActualEndAt.Should().Be(DateTime.MinValue);
    }

    [Theory]
    [InlineData(HouseKeepingTaskStatus.Completed)]
    [InlineData(HouseKeepingTaskStatus.Cancelled)]
    public void StartActualTimeInterval_ShouldThrowBusinessRuleException_WhenStatusDoesNotAllowChanges(HouseKeepingTaskStatus status)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.UpdatedStatus(status);

        // Act
        Action act = () => houseKeepingTask.StartActualTimeInterval();

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void EndActualTimeInterval_ShouldUpdateActualTimeInterval_WhenStatusAllowsIt()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        var currentActualTimeInterval = new TaskActualTimeInterval(
            new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
            DateTime.MinValue);

        // Act
        houseKeepingTask.EndActualTimeInterval(currentActualTimeInterval);

        // Assert
        houseKeepingTask.ActualTimeInterval.Should().NotBeNull();
        houseKeepingTask.ActualTimeInterval!.ActualStartAt.Should().Be(currentActualTimeInterval.ActualStartAt);
        houseKeepingTask.ActualTimeInterval.ActualEndAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }

    [Theory]
    [InlineData(HouseKeepingTaskStatus.Completed)]
    [InlineData(HouseKeepingTaskStatus.Cancelled)]
    public void EndActualTimeInterval_ShouldThrowBusinessRuleException_WhenStatusDoesNotAllowChanges(HouseKeepingTaskStatus status)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.UpdatedStatus(status);
        var currentActualTimeInterval = new TaskActualTimeInterval(
            new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
            DateTime.MinValue);

        // Act
        Action act = () => houseKeepingTask.EndActualTimeInterval(currentActualTimeInterval);

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Fact]
    public void ReportIncident_ShouldSetIncidentDetails_WhenValueIsValid()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        houseKeepingTask.ReportIncident("Broken lamp found during inspection");

        // Assert
        houseKeepingTask.IncidentReported.Should().BeTrue();
        houseKeepingTask.IncidentDescription.Should().Be("Broken lamp found during inspection");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void ReportIncident_ShouldThrowEmptyRequiredFieldException_WhenDescriptionIsNullOrWhiteSpace(string? incidentDescription)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        Action act = () => houseKeepingTask.ReportIncident(incidentDescription!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    [Fact]
    public void UpdateIncidentDescription_ShouldUpdateIncidentDescription_WhenIncidentWasReported()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.ReportIncident("Broken lamp found during inspection");

        // Act
        houseKeepingTask.UpdateIncidentDescription("Broken lamp and damaged shade found during inspection");

        // Assert
        houseKeepingTask.IncidentDescription.Should().Be("Broken lamp and damaged shade found during inspection");
    }

    [Fact]
    public void UpdateIncidentDescription_ShouldThrowBusinessRuleException_WhenIncidentWasNotReported()
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();

        // Act
        Action act = () => houseKeepingTask.UpdateIncidentDescription("New incident details");

        // Assert
        act.Should().Throw<BusinessRuleException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void UpdateIncidentDescription_ShouldThrowEmptyRequiredFieldException_WhenValueIsNullOrWhiteSpace(string? newIncidentDescription)
    {
        // Arrange
        var houseKeepingTask = CreateHouseKeepingTask();
        houseKeepingTask.ReportIncident("Broken lamp found during inspection");

        // Act
        Action act = () => houseKeepingTask.UpdateIncidentDescription(newIncidentDescription!);

        // Assert
        act.Should().Throw<EmptyRequiredFieldException>();
    }

    private static HouseKeepingTask CreateHouseKeepingTask()
    {
        return new HouseKeepingTask(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            HouseKeepingTaskType.Cleaning,
            TaskPriority.High,
            CreateExpectedTimeInterval(),
            "Clean room before next guest arrival");
    }

    private static TaskExpectedTimeInterval CreateExpectedTimeInterval()
    {
        return new TaskExpectedTimeInterval(
            new DateTime(2026, 4, 1, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(2026, 4, 1, 11, 0, 0, DateTimeKind.Utc));
    }
}
